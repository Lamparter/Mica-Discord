namespace MicaDiscord
{
    partial class WinForms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WebView = new MicaDiscord.WebView3();
            this.Back = new MicaDiscord.ThemeButton();
            this.Forward = new MicaDiscord.ThemeButton();
            this.themeLabel1 = new MicaDiscord.ThemeLabel();
            this.CloseBtn = new MicaDiscord.ThemeButton();
            this.Maximize = new MicaDiscord.ThemeButton();
            this.Minimize = new MicaDiscord.ThemeButton();
            this.themeButton4 = new MicaDiscord.ThemeButton();
            this.themeButton5 = new MicaDiscord.ThemeButton();
            this.themeLabel2 = new MicaDiscord.ThemeLabel();
            this.OtherTitleBarRegion = new MicaDiscord.ThemeLabel();
            ((System.ComponentModel.ISupportInitialize)(this.WebView)).BeginInit();
            this.SuspendLayout();
            // 
            // WebView
            // 
            this.WebView.AllowExternalDrop = true;
            this.WebView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WebView.CreationProperties = null;
            this.WebView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            this.WebView.Location = new System.Drawing.Point(7, 7);
            this.WebView.Margin = new System.Windows.Forms.Padding(0);
            this.WebView.Name = "WebView";
            this.WebView.Size = new System.Drawing.Size(786, 436);
            this.WebView.Source = new System.Uri("https://discord.com/channels/@me", System.UriKind.Absolute);
            this.WebView.TabIndex = 0;
            this.WebView.ZoomFactor = 1D;
            // 
            // Back
            // 
            this.Back.BackColor = System.Drawing.Color.Transparent;
            this.Back.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.Back.ContentText = "B";
            this.Back.ForeColor = System.Drawing.Color.White;
            this.Back.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Back.Location = new System.Drawing.Point(86, 7);
            this.Back.Margin = new System.Windows.Forms.Padding(0);
            this.Back.Name = "Back";
            this.Back.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Back.Size = new System.Drawing.Size(30, 30);
            this.Back.TabIndex = 1;
            // 
            // Forward
            // 
            this.Forward.BackColor = System.Drawing.Color.Transparent;
            this.Forward.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.Forward.ContentText = "F";
            this.Forward.ForeColor = System.Drawing.Color.White;
            this.Forward.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Forward.Location = new System.Drawing.Point(116, 7);
            this.Forward.Margin = new System.Windows.Forms.Padding(0);
            this.Forward.Name = "Forward";
            this.Forward.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Forward.Size = new System.Drawing.Size(30, 30);
            this.Forward.TabIndex = 2;
            // 
            // themeLabel1
            // 
            this.themeLabel1.BackColor = System.Drawing.Color.Transparent;
            this.themeLabel1.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.themeLabel1.ContentText = "Mica Discord";
            this.themeLabel1.ForeColor = System.Drawing.Color.White;
            this.themeLabel1.Location = new System.Drawing.Point(7, 7);
            this.themeLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.themeLabel1.Name = "themeLabel1";
            this.themeLabel1.Size = new System.Drawing.Size(75, 30);
            this.themeLabel1.TabIndex = 3;
            this.themeLabel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseMove);
            this.themeLabel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseDoubleClick);
            // 
            // Close
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseBtn.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.CloseBtn.ContentText = "X";
            this.CloseBtn.ForeColor = System.Drawing.Color.White;
            this.CloseBtn.HoverColor = System.Drawing.Color.Red;
            this.CloseBtn.Location = new System.Drawing.Point(763, 7);
            this.CloseBtn.Margin = new System.Windows.Forms.Padding(0);
            this.CloseBtn.Name = "Close";
            this.CloseBtn.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CloseBtn.Size = new System.Drawing.Size(30, 30);
            this.CloseBtn.TabIndex = 4;
            // 
            // Maximize
            // 
            this.Maximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Maximize.BackColor = System.Drawing.Color.Transparent;
            this.Maximize.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.Maximize.ContentText = "▭";
            this.Maximize.ForeColor = System.Drawing.Color.White;
            this.Maximize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Maximize.Location = new System.Drawing.Point(733, 7);
            this.Maximize.Margin = new System.Windows.Forms.Padding(0);
            this.Maximize.Name = "Maximize";
            this.Maximize.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Maximize.Size = new System.Drawing.Size(30, 30);
            this.Maximize.TabIndex = 5;
            // 
            // Minimize
            // 
            this.Minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Minimize.BackColor = System.Drawing.Color.Transparent;
            this.Minimize.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.Minimize.ContentText = "-";
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Minimize.Location = new System.Drawing.Point(703, 7);
            this.Minimize.Margin = new System.Windows.Forms.Padding(0);
            this.Minimize.Name = "Minimize";
            this.Minimize.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Minimize.Size = new System.Drawing.Size(30, 30);
            this.Minimize.TabIndex = 6;
            // 
            // themeButton4
            // 
            this.themeButton4.BackColor = System.Drawing.Color.Transparent;
            this.themeButton4.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.themeButton4.ContentText = "R";
            this.themeButton4.ForeColor = System.Drawing.Color.White;
            this.themeButton4.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.themeButton4.Location = new System.Drawing.Point(146, 7);
            this.themeButton4.Margin = new System.Windows.Forms.Padding(0);
            this.themeButton4.Name = "themeButton4";
            this.themeButton4.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.themeButton4.Size = new System.Drawing.Size(30, 30);
            this.themeButton4.TabIndex = 7;
            // 
            // themeButton5
            // 
            this.themeButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.themeButton5.BackColor = System.Drawing.Color.Transparent;
            this.themeButton5.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.themeButton5.ContentText = "S";
            this.themeButton5.ForeColor = System.Drawing.Color.White;
            this.themeButton5.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.themeButton5.Location = new System.Drawing.Point(673, 7);
            this.themeButton5.Margin = new System.Windows.Forms.Padding(0);
            this.themeButton5.Name = "themeButton5";
            this.themeButton5.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.themeButton5.Size = new System.Drawing.Size(30, 30);
            this.themeButton5.TabIndex = 8;
            // 
            // themeLabel2
            // 
            this.themeLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.themeLabel2.BackColor = System.Drawing.Color.Transparent;
            this.themeLabel2.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.themeLabel2.ContentText = "general";
            this.themeLabel2.ForeColor = System.Drawing.Color.White;
            this.themeLabel2.Location = new System.Drawing.Point(183, 7);
            this.themeLabel2.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.themeLabel2.Name = "themeLabel2";
            this.themeLabel2.Size = new System.Drawing.Size(490, 30);
            this.themeLabel2.TabIndex = 9;
            this.themeLabel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseMove);
            this.themeLabel2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseDoubleClick);
            // 
            // OtherTitleBarRegion
            // 
            this.OtherTitleBarRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherTitleBarRegion.BackColor = System.Drawing.Color.Transparent;
            this.OtherTitleBarRegion.ContentAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.OtherTitleBarRegion.ContentText = "";
            this.OtherTitleBarRegion.ForeColor = System.Drawing.Color.White;
            this.OtherTitleBarRegion.Location = new System.Drawing.Point(7, 7);
            this.OtherTitleBarRegion.Margin = new System.Windows.Forms.Padding(0);
            this.OtherTitleBarRegion.Name = "OtherTitleBarRegion";
            this.OtherTitleBarRegion.Size = new System.Drawing.Size(786, 30);
            this.OtherTitleBarRegion.TabIndex = 10;
            this.OtherTitleBarRegion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseMove);
            this.OtherTitleBarRegion.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TitleBarMouseDoubleClick);
            // 
            // WinForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.themeLabel2);
            this.Controls.Add(this.themeButton5);
            this.Controls.Add(this.themeButton4);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.Maximize);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.themeLabel1);
            this.Controls.Add(this.Forward);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.OtherTitleBarRegion);
            this.Controls.Add(this.WebView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinForms";
            this.Text = "Mica Discord";
            ((System.ComponentModel.ISupportInitialize)(this.WebView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WebView3 WebView;
        private ThemeButton Back;
        private ThemeButton Forward;
        private ThemeLabel themeLabel1;
        private ThemeButton CloseBtn;
        private ThemeButton Maximize;
        private ThemeButton Minimize;
        private ThemeButton themeButton4;
        private ThemeButton themeButton5;
        private ThemeLabel themeLabel2;
        private ThemeLabel OtherTitleBarRegion;
    }
}