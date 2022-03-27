﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class DashboardForm : Form, ITournamentRequestor
    {
        private BindingList<TournamentModel> tournaments = new BindingList<TournamentModel>();

        public DashboardForm()
        {
            InitializeComponent();

            InitializeFormData();
        }

        private void InitializeFormData()
        {
            tournaments = new BindingList<TournamentModel>(GlobalConfig.connection.GetTournament_All().FindAll(x => x.Active));
            tournamentListBox.DataSource = tournaments;
            tournamentListBox.DisplayMember = "TournamentName";
        }

        private void CreateTournamentButton_Click(object sender, EventArgs e)
        {
            NewTournamentForm frm = new NewTournamentForm(this);
            frm.Show();

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        public void NewTournamentComplete(TournamentModel tournament)
        {
            tournaments.Add(tournament);

            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void ViewTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tournament = (TournamentModel)tournamentListBox.SelectedItem;

            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            TournamentViewerForm frm = new TournamentViewerForm(tournament, this);
            frm.Show();

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            InitializeFormData();
        }
    }
}
