extern alias WV2;
namespace MicaDiscord;

partial class ModernContextMenuForm
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
        this.WebView = new WV2::Microsoft.Web.WebView2.WinForms.WebView2();
        ((System.ComponentModel.ISupportInitialize)(this.WebView)).BeginInit();
        this.SuspendLayout();
        // 
        // WebView
        // 
        this.WebView.AllowExternalDrop = true;
        this.WebView.CreationProperties = null;
        this.WebView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
        this.WebView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.WebView.Location = new System.Drawing.Point(0, 0);
        this.WebView.Margin = new System.Windows.Forms.Padding(0);
        this.WebView.Name = "WebView";
        this.WebView.Size = new System.Drawing.Size(2, 2);
        this.WebView.Source = new System.Uri("about:blank", System.UriKind.Absolute);
        this.WebView.TabIndex = 0;
        this.WebView.ZoomFactor = 1D;
        // 
        // ModernContextMenuForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Yellow;
        this.ClientSize = new System.Drawing.Size(2, 2);
        this.ControlBox = false;
        this.Controls.Add(this.WebView);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ModernContextMenuForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.TransparencyKey = System.Drawing.Color.Yellow;
        ((System.ComponentModel.ISupportInitialize)(this.WebView)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private WV2::Microsoft.Web.WebView2.WinForms.WebView2 WebView;
}
