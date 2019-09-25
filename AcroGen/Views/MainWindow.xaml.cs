using System;
using System.Windows;
using AcroGen.DomainModels;
using AcroGen.ViewModels;
using Hurst.BaseLib;
using Hurst.BaseLibWpf;
using Hurst.BaseLibWpf.DialogWindows;


namespace AcroGen.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = ApplicationViewModel.The;
            ApplicationViewModel.EditOptionsRequested += OnEditOptionsRequested;
            ApplicationViewModel.SelectDocDefinitionFilePathnameRequested += OnSelectDocDefinitionFilePathnameRequested;
            ApplicationViewModel.ShowHelpAboutMeRequested += OnShowHelpAboutMeRequested;
            ApplicationViewModel.UpdateDocumentDisplayRequested += OnUpdateDocumentDisplayRequested;
            ApplicationViewModel.UserNotificationRequested += OnUserNotificationRequested;
            LayoutUpdated += OnLayoutUpdated;
            Closing += OnClosing;
        }

        #region UserSettings
        /// <summary>
        /// Get the singleton-instance of the user-set configuration-settings for this program.
        /// </summary>
        public AcrogenUserSettings UserSettings
        {
            get { return AcrogenUserSettings.The; }
        }
        #endregion

        #region OnLayoutUpdated
        /// <summary>
        /// Handle the LayoutUpdated event by setting the position of this application on the user's desktop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLayoutUpdated( object sender, EventArgs e )
        {
            // Remember the original window extent, in case the user later wants to reset it back to the original designed values.
            if (!_hasOriginalExtentBeenSaved)
            {
                _windowHeightOriginal = this.Height;
                _windowWidthOriginal = this.Width;
                _hasOriginalExtentBeenSaved = true;
            }
            UserSettings.SetWindowToSavedPosition( this );
        }
        #endregion

        #region OnUpdateDocumentDisplayRequested
        /// <summary>
        /// Handle the UpdateDocumentDisplayRequested event by either showing the document, or updating it
        /// if it is already being shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdateDocumentDisplayRequested( object sender, EventArgs e )
        {
            if (_viewModel.DocDefinitionFilePathname.HasNothing())
            {
                _viewModel.NotifyUserOfMistake( message: "You need to set the pathname of the file first." );
                return;
            }

            if (_docCreator == null)
            {
                _docCreator = new DocCreator();
            }
            _docCreator.Save( path: _viewModel.DocDefinitionFilePathname );
        }

        private DocCreator _docCreator;
        #endregion

        #region OnEditOptionsRequested
        /// <summary>
        /// Handle the EditOptionsRequested event by bringing up the Options dialog-window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditOptionsRequested( object sender, EventArgs e )
        {
            var editOptionsWindow = new OptionsWindow();
            editOptionsWindow.Owner = this;
            var r = editOptionsWindow.ShowDialog();
        }
        #endregion

        private void OnSelectDocDefinitionFilePathnameRequested( object sender, EventArgs e )
        {
            var pathnameSelector = new FileOpenDialog();
            pathnameSelector.Title = "Specify the pathname of the file containing the document-definition.";
            //  pathnameSelector.InitialDirectory = _viewModel.DocDefinitionFilePathname;
            if (StringLib.HasNothing( _viewModel.DocDefinitionFilePathname ))
            {
                // pathnameSelector.SelectedFilename = "CodeMetricsComparisonReport.txt";
            }
            var r = pathnameSelector.ShowDialog();
            if (r == DisplayUxResult.Ok)
            {
                _viewModel.DocDefinitionFilePathname = pathnameSelector.SelectedFilename;
            }
        }

        #region OnShowHelpAboutMeRequested
        /// <summary>
        /// Handle the ShowHelpAboutMeRequested event by bringing up the 'Help/About' dialog-window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowHelpAboutMeRequested( object sender, EventArgs e )
        {
            var dialogWindow = new AboutWindow();
            dialogWindow.Owner = this;
            dialogWindow.ShowDialog();
        }
        #endregion

        #region OnUserNotificationRequested
        /// <summary>
        /// Handle the UserNotificationRequested event by showing the indicated notification message to the end-user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserNotificationRequested( object sender, UserNotificationEventArgs e )
        {
            if (e.IsUserMistake)
            {
                App.TheLogger.LogWarning( e.UserMessage );
                App.The.Interlocution.NotifyUserOfMistake( e.UserMessage );
            }
            else if (e.IsError)
            {
                App.TheLogger.LogError( e.UserMessage );
                App.The.Interlocution.NotifyUserOfError( e.UserMessage );
            }
            else if (e.IsWarning)
            {
                App.The.Interlocution.WarnUser( e.UserMessage );
            }
            else
            {
                App.The.Interlocution.NotifyUser( e.UserMessage );
            }
        }
        #endregion

        #region OnClosing
        /// <summary>
        /// Handle the Closing event of the main-window
        /// by saving any information that the operator has entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            //  _viewModel?.Save();
            UserSettings.SaveWindowPositionIfChanged( this );
            UserSettings.SaveIfChanged();
        }
        #endregion

        #region fields

        /// <summary>
        /// This flag serves to cause the LayoutUpdated event - which gets raised every time the window is moved - to set the saved extent only once.
        /// </summary>
        private bool _hasOriginalExtentBeenSaved;

        private readonly ApplicationViewModel _viewModel;
        /// <summary>
        /// This serves to hold the original design height of the main-window, in case the user wants to reset it back to this.
        /// </summary>
        private double _windowHeightOriginal;

        /// <summary>
        /// This serves to hold the original design width of the main-window, in case the user wants to reset it back to this.
        /// </summary>
        private double _windowWidthOriginal;

        #endregion fields
    }
}
