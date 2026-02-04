using BattleshipLab3.Game;
using BattleshipLab3.Models;
using BattleshipLab3.Network;

namespace BattleshipLab3;

public partial class Form1 : Form
{
    private readonly Button[,] _playerButtons = new Button[Board.Size, Board.Size];
    private readonly Button[,] _enemyButtons = new Button[Board.Size, Board.Size];

    private GameState? _gameState;
    private NetworkSession? _session;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        BuildGrid(playerGrid, _playerButtons, PlayerCell_Click);
        BuildGrid(enemyGrid, _enemyButtons, EnemyCell_Click);
        SetUiEnabled(false);
    }

    private void BuildGrid(TableLayoutPanel grid, Button[,] buttons, EventHandler clickHandler)
    {
        grid.Controls.Clear();
        grid.SuspendLayout();

        for (int y = 0; y < Board.Size; y++)
        {
            for (int x = 0; x < Board.Size; x++)
            {
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(1),
                    Tag = (x, y),
                    BackColor = Color.LightBlue
                };
                button.Click += clickHandler;
                buttons[x, y] = button;
                grid.Controls.Add(button, x, y);
            }
        }

        grid.ResumeLayout();
    }

    private async void btnHost_Click(object? sender, EventArgs e)
    {
        int port = (int)numPort.Value;
        _gameState = new GameState(isHost: true);
        _session = new NetworkSession(isHost: true, hostAddress: "0.0.0.0", port: port);
        SubscribeToSession();

        lblStatus.Text = "Вы хост. Ожидание клиента...";
        await _session.StartAsync();
        _gameState.StartPlacement();
        SetUiEnabled(true);
        lblStatus.Text = "Расставьте корабли и нажмите Старт";
    }

    private async void btnJoin_Click(object? sender, EventArgs e)
    {
        string ip = string.IsNullOrWhiteSpace(txtIp.Text) ? "127.0.0.1" : txtIp.Text.Trim();
        int port = (int)numPort.Value;
        _gameState = new GameState(isHost: false);
        _session = new NetworkSession(isHost: false, hostAddress: ip, port: port);
        SubscribeToSession();

        lblStatus.Text = "Подключение к хосту...";
        await _session.StartAsync();
        _gameState.StartPlacement();
        SetUiEnabled(true);
        lblStatus.Text = "Расставьте корабли и ждите начала игры";
    }

    private void SubscribeToSession()
    {
        if (_session == null)
            return;

        _session.MessageReceived += OnNetworkMessageReceived;
        _session.ConnectionStatusChanged += text =>
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => lblStatus.Text = text));
            }
            else
            {
                lblStatus.Text = text;
            }
        };
    }

    private void SetUiEnabled(bool afterConnection)
    {
        btnAutoPlace.Enabled = afterConnection;
        btnStartGame.Enabled = afterConnection;
        enemyGrid.Enabled = false;
    }

    private void PlayerCell_Click(object? sender, EventArgs e)
    {
        if (_gameState == null)
            return;

        if (_gameState.Phase != GamePhase.Placement)
            return;

        // В этой реализации ручная расстановка не делается,
        // используем кнопку "Автофлот".
    }

    private async void EnemyCell_Click(object? sender, EventArgs e)
    {
        if (_gameState == null || _session == null)
            return;

        if (_gameState.Phase != GamePhase.MyTurn)
            return;

        if (sender is not Button button)
            return;

        var (x, y) = ((int x, int y))button.Tag;

        await _session.SendAsync(new NetworkMessage
        {
            Type = MessageType.Shot,
            X = x,
            Y = y
        });

        _gameState.SetPhase(GamePhase.EnemyTurn);
        UpdateStatusText();
    }

    private void btnAutoPlace_Click(object? sender, EventArgs e)
    {
        if (_gameState == null)
            return;

        _gameState.MyBoard.AutoPlaceStandardFleet();
        RedrawBoards();
    }

    private async void btnStartGame_Click(object? sender, EventArgs e)
    {
        if (_gameState == null || _session == null)
            return;

        if (_gameState.Phase != GamePhase.Placement)
            return;

        await _session.SendAsync(new NetworkMessage
        {
            Type = MessageType.StartGame
        });

        if (_gameState.IsHost)
            _gameState.StartGame(myTurnFirst: true);

        enemyGrid.Enabled = _gameState.Phase == GamePhase.MyTurn;
        UpdateStatusText();
    }

    private async void btnSendChat_Click(object? sender, EventArgs e)
    {
        if (_session == null)
            return;

        string text = txtChatInput.Text.Trim();
        if (string.IsNullOrEmpty(text))
            return;

        txtChatInput.Clear();
        lstChat.Items.Add("Вы: " + text);

        await _session.SendAsync(new NetworkMessage
        {
            Type = MessageType.Chat,
            Message = text
        });
    }

    private void OnNetworkMessageReceived(NetworkMessage message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnNetworkMessageReceived(message)));
            return;
        }

        if (_gameState == null)
            return;

        switch (message.Type)
        {
            case MessageType.StartGame:
                if (!_gameState.IsHost)
                    _gameState.StartGame(myTurnFirst: false);
                enemyGrid.Enabled = _gameState.Phase == GamePhase.MyTurn;
                UpdateStatusText();
                break;

            case MessageType.Shot:
                HandleIncomingShot(message);
                break;

            case MessageType.ShotResult:
                HandleShotResult(message);
                break;

            case MessageType.Chat:
                if (!string.IsNullOrEmpty(message.Message))
                    lstChat.Items.Add("Соперник: " + message.Message);
                break;
        }
    }

    private async void HandleIncomingShot(NetworkMessage message)
    {
        if (_gameState == null || _session == null)
            return;

        if (message.X == null || message.Y == null)
            return;

        int x = message.X.Value;
        int y = message.Y.Value;

        var result = _gameState.MyBoard.ShootAt(x, y);
        RedrawBoards();

        await _session.SendAsync(new NetworkMessage
        {
            Type = MessageType.ShotResult,
            X = x,
            Y = y,
            Outcome = result.Outcome.ToString()
        });

        if (_gameState.MyBoard.AllShipsSunk())
        {
            _gameState.FinishGame();
            lblStatus.Text = "Поражение";
            enemyGrid.Enabled = false;
            return;
        }

        if (result.Outcome == ShotOutcome.Miss || result.Outcome == ShotOutcome.Repeated)
        {
            _gameState.SetPhase(GamePhase.MyTurn);
            enemyGrid.Enabled = true;
        }
        else
        {
            _gameState.SetPhase(GamePhase.EnemyTurn);
        }

        UpdateStatusText();
    }

    private void HandleShotResult(NetworkMessage message)
    {
        if (_gameState == null)
            return;

        if (message.X == null || message.Y == null || string.IsNullOrEmpty(message.Outcome))
            return;

        int x = message.X.Value;
        int y = message.Y.Value;

        if (!Enum.TryParse<ShotOutcome>(message.Outcome, out var outcome))
            return;

        var current = _gameState.EnemyBoard[x, y];
        switch (outcome)
        {
            case ShotOutcome.Miss:
                if (current == CellState.Empty)
                    SetEnemyCellState(x, y, CellState.Miss);
                _gameState.SetPhase(GamePhase.EnemyTurn);
                enemyGrid.Enabled = false;
                break;
            case ShotOutcome.Hit:
                SetEnemyCellState(x, y, CellState.Hit);
                _gameState.SetPhase(GamePhase.MyTurn);
                enemyGrid.Enabled = true;
                break;
            case ShotOutcome.Sunk:
                SetEnemyCellState(x, y, CellState.Sunk);
                _gameState.SetPhase(GamePhase.MyTurn);
                enemyGrid.Enabled = true;
                break;
            case ShotOutcome.Repeated:
                _gameState.SetPhase(GamePhase.EnemyTurn);
                enemyGrid.Enabled = false;
                break;
        }

        if (_gameState.EnemyBoard.AllShipsSunk())
        {
            _gameState.FinishGame();
            enemyGrid.Enabled = false;
            lblStatus.Text = "Победа";
            return;
        }

        UpdateStatusText();
    }

    private void SetEnemyCellState(int x, int y, CellState state)
    {
        var button = _enemyButtons[x, y];
        button.BackColor = state switch
        {
            CellState.Miss => Color.LightGray,
            CellState.Hit => Color.OrangeRed,
            CellState.Sunk => Color.DarkRed,
            _ => button.BackColor
        };
    }

    private void RedrawBoards()
    {
        if (_gameState == null)
            return;

        for (int x = 0; x < Board.Size; x++)
        {
            for (int y = 0; y < Board.Size; y++)
            {
                var myCell = _gameState.MyBoard[x, y];
                var myButton = _playerButtons[x, y];

                myButton.BackColor = myCell switch
                {
                    CellState.Empty => Color.LightBlue,
                    CellState.Ship => Color.SteelBlue,
                    CellState.Miss => Color.LightGray,
                    CellState.Hit => Color.OrangeRed,
                    CellState.Sunk => Color.DarkRed,
                    _ => myButton.BackColor
                };
            }
        }
    }

    private void UpdateStatusText()
    {
        if (_gameState == null)
            return;

        lblStatus.Text = _gameState.Phase switch
        {
            GamePhase.WaitingForConnection => "Ожидание подключения",
            GamePhase.Placement => "Расстановка флота",
            GamePhase.MyTurn => "Ваш ход",
            GamePhase.EnemyTurn => "Ход соперника",
            GamePhase.GameOver => "Игра окончена",
            _ => lblStatus.Text
        };
    }
}
