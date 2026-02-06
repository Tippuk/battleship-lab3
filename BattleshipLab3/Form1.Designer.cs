namespace BattleshipLab3;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel playerGrid;
    private TableLayoutPanel enemyGrid;
    private Button btnHost;
    private Button btnJoin;
    private TextBox txtIp;
    private NumericUpDown numPort;
    private Label lblStatus;
    private Button btnAutoPlace;
    private Button btnStartGame;
    private Button btnRestart;
    private TextBox txtChatInput;
    private Button btnSendChat;
    private ListBox lstChat;
    private Label lblPlayerField;
    private Label lblEnemyField;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        playerGrid = new TableLayoutPanel();
        enemyGrid = new TableLayoutPanel();
        btnHost = new Button();
        btnJoin = new Button();
        txtIp = new TextBox();
        numPort = new NumericUpDown();
        lblStatus = new Label();
        btnAutoPlace = new Button();
        btnStartGame = new Button();
        btnRestart = new Button();
        txtChatInput = new TextBox();
        btnSendChat = new Button();
        lstChat = new ListBox();
        lblPlayerField = new Label();
        lblEnemyField = new Label();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        SuspendLayout();
        // 
        // playerGrid
        // 
        playerGrid.ColumnCount = 10;
        playerGrid.RowCount = 10;
        for (int i = 0; i < 10; i++)
        {
            playerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            playerGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
        }
        playerGrid.Location = new Point(12, 40);
        playerGrid.Name = "playerGrid";
        playerGrid.Size = new Size(300, 300);
        playerGrid.TabIndex = 0;
        // 
        // enemyGrid
        // 
        enemyGrid.ColumnCount = 10;
        enemyGrid.RowCount = 10;
        for (int i = 0; i < 10; i++)
        {
            enemyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            enemyGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
        }
        enemyGrid.Location = new Point(330, 40);
        enemyGrid.Name = "enemyGrid";
        enemyGrid.Size = new Size(300, 300);
        enemyGrid.TabIndex = 1;
        // 
        // btnHost
        // 
        btnHost.Location = new Point(12, 360);
        btnHost.Name = "btnHost";
        btnHost.Size = new Size(100, 30);
        btnHost.TabIndex = 2;
        btnHost.Text = "Host";
        btnHost.UseVisualStyleBackColor = true;
        btnHost.Click += btnHost_Click;
        // 
        // btnJoin
        // 
        btnJoin.Location = new Point(118, 360);
        btnJoin.Name = "btnJoin";
        btnJoin.Size = new Size(100, 30);
        btnJoin.TabIndex = 3;
        btnJoin.Text = "Join";
        btnJoin.UseVisualStyleBackColor = true;
        btnJoin.Click += btnJoin_Click;
        // 
        // txtIp
        // 
        txtIp.Location = new Point(224, 364);
        txtIp.Name = "txtIp";
        txtIp.PlaceholderText = "IP";
        txtIp.Size = new Size(120, 23);
        txtIp.TabIndex = 4;
        // 
        // numPort
        // 
        numPort.Location = new Point(350, 364);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(80, 23);
        numPort.TabIndex = 5;
        numPort.Value = new decimal(new int[] { 9000, 0, 0, 0 });
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(12, 9);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(90, 15);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "Ожидание игры";
        // 
        // btnAutoPlace
        // 
        btnAutoPlace.Location = new Point(450, 360);
        btnAutoPlace.Name = "btnAutoPlace";
        btnAutoPlace.Size = new Size(90, 30);
        btnAutoPlace.TabIndex = 7;
        btnAutoPlace.Text = "Автофлот";
        btnAutoPlace.UseVisualStyleBackColor = true;
        btnAutoPlace.Click += btnAutoPlace_Click;
        // 
        // btnStartGame
        // 
        btnStartGame.Location = new Point(546, 360);
        btnStartGame.Name = "btnStartGame";
        btnStartGame.Size = new Size(84, 30);
        btnStartGame.TabIndex = 8;
        btnStartGame.Text = "Старт";
        btnStartGame.UseVisualStyleBackColor = true;
        btnStartGame.Click += btnStartGame_Click;
        // 
        // btnRestart
        // 
        btnRestart.Location = new Point(636, 360);
        btnRestart.Name = "btnRestart";
        btnRestart.Size = new Size(100, 30);
        btnRestart.TabIndex = 9;
        btnRestart.Text = "Новый раунд";
        btnRestart.UseVisualStyleBackColor = true;
        btnRestart.Enabled = false;
        btnRestart.Click += btnRestart_Click;
        // 
        // txtChatInput
        // 
        txtChatInput.Location = new Point(12, 410);
        txtChatInput.Name = "txtChatInput";
        txtChatInput.Size = new Size(300, 23);
        txtChatInput.TabIndex = 9;
        // 
        // btnSendChat
        // 
        btnSendChat.Location = new Point(318, 408);
        btnSendChat.Name = "btnSendChat";
        btnSendChat.Size = new Size(75, 27);
        btnSendChat.TabIndex = 10;
        btnSendChat.Text = "Отправить";
        btnSendChat.UseVisualStyleBackColor = true;
        btnSendChat.Click += btnSendChat_Click;
        // 
        // lstChat
        // 
        lstChat.FormattingEnabled = true;
        lstChat.ItemHeight = 15;
        lstChat.Location = new Point(630, 40);
        lstChat.Name = "lstChat";
        lstChat.Size = new Size(200, 319);
        lstChat.TabIndex = 11;
        // 
        // lblPlayerField
        // 
        lblPlayerField.AutoSize = true;
        lblPlayerField.Location = new Point(12, 22);
        lblPlayerField.Name = "lblPlayerField";
        lblPlayerField.Size = new Size(70, 15);
        lblPlayerField.TabIndex = 12;
        lblPlayerField.Text = "Ваше поле";
        // 
        // lblEnemyField
        // 
        lblEnemyField.AutoSize = true;
        lblEnemyField.Location = new Point(330, 22);
        lblEnemyField.Name = "lblEnemyField";
        lblEnemyField.Size = new Size(75, 15);
        lblEnemyField.TabIndex = 13;
        lblEnemyField.Text = "Поле врага";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(844, 451);
        Controls.Add(lblEnemyField);
        Controls.Add(lblPlayerField);
        Controls.Add(lstChat);
        Controls.Add(btnSendChat);
        Controls.Add(txtChatInput);
        Controls.Add(btnRestart);
        Controls.Add(btnStartGame);
        Controls.Add(btnAutoPlace);
        Controls.Add(lblStatus);
        Controls.Add(numPort);
        Controls.Add(txtIp);
        Controls.Add(btnJoin);
        Controls.Add(btnHost);
        Controls.Add(enemyGrid);
        Controls.Add(playerGrid);
        Name = "Form1";
        Text = "Морской бой";
        FormClosing += Form1_FormClosing;
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}

