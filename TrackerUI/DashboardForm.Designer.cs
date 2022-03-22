
namespace TrackerUI
{
    partial class DashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            this.dashboardHeaderLabel = new System.Windows.Forms.Label();
            this.tournamentListBox = new System.Windows.Forms.ListBox();
            this.viewTournamentButton = new System.Windows.Forms.Button();
            this.deleteTournamentButton = new System.Windows.Forms.Button();
            this.createTournamentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dashboardHeaderLabel
            // 
            this.dashboardHeaderLabel.AutoSize = true;
            this.dashboardHeaderLabel.Font = new System.Drawing.Font("Segoe UI Light", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashboardHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.dashboardHeaderLabel.Location = new System.Drawing.Point(149, 9);
            this.dashboardHeaderLabel.Name = "dashboardHeaderLabel";
            this.dashboardHeaderLabel.Size = new System.Drawing.Size(387, 60);
            this.dashboardHeaderLabel.TabIndex = 0;
            this.dashboardHeaderLabel.Text = "Tournament Tracker";
            this.dashboardHeaderLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tournamentListBox
            // 
            this.tournamentListBox.FormattingEnabled = true;
            this.tournamentListBox.ItemHeight = 36;
            this.tournamentListBox.Location = new System.Drawing.Point(27, 90);
            this.tournamentListBox.Name = "tournamentListBox";
            this.tournamentListBox.Size = new System.Drawing.Size(248, 220);
            this.tournamentListBox.TabIndex = 1;
            // 
            // viewTournamentButton
            // 
            this.viewTournamentButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewTournamentButton.Location = new System.Drawing.Point(365, 130);
            this.viewTournamentButton.Name = "viewTournamentButton";
            this.viewTournamentButton.Size = new System.Drawing.Size(182, 45);
            this.viewTournamentButton.TabIndex = 2;
            this.viewTournamentButton.Text = "View Tournament";
            this.viewTournamentButton.UseVisualStyleBackColor = true;
            // 
            // deleteTournamentButton
            // 
            this.deleteTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.deleteTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.deleteTournamentButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteTournamentButton.Location = new System.Drawing.Point(365, 217);
            this.deleteTournamentButton.Name = "deleteTournamentButton";
            this.deleteTournamentButton.Size = new System.Drawing.Size(182, 45);
            this.deleteTournamentButton.TabIndex = 3;
            this.deleteTournamentButton.Text = "Delete Tournament";
            this.deleteTournamentButton.UseVisualStyleBackColor = true;
            // 
            // createTournamentButton
            // 
            this.createTournamentButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createTournamentButton.Location = new System.Drawing.Point(176, 384);
            this.createTournamentButton.Name = "createTournamentButton";
            this.createTournamentButton.Size = new System.Drawing.Size(245, 45);
            this.createTournamentButton.TabIndex = 4;
            this.createTournamentButton.Text = "Create Tournament";
            this.createTournamentButton.UseVisualStyleBackColor = true;
            this.createTournamentButton.Click += new System.EventHandler(this.createTournamentButton_Click);
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.createTournamentButton);
            this.Controls.Add(this.deleteTournamentButton);
            this.Controls.Add(this.viewTournamentButton);
            this.Controls.Add(this.tournamentListBox);
            this.Controls.Add(this.dashboardHeaderLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "DashboardForm";
            this.Text = "Tournament Tracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label dashboardHeaderLabel;
        private System.Windows.Forms.ListBox tournamentListBox;
        private System.Windows.Forms.Button viewTournamentButton;
        private System.Windows.Forms.Button deleteTournamentButton;
        private System.Windows.Forms.Button createTournamentButton;
    }
}