using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Hurst.BaseLib;
using Hurst.BaseLibWpf;


namespace AcroGen.ViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        #region events

        public static event EventHandler<EventArgs> EditOptionsRequested;
        public static event EventHandler<EventArgs> SelectDocDefinitionFilePathnameRequested;

        public static event EventHandler<EventArgs> ShowHelpAboutMeRequested;

        /// <summary>
        /// This event signifies that the user wants to update the Acrobat document as it displayed
        /// on-screen, or starting displaying it if it is not yet being shown.
        /// </summary>
        public static event EventHandler<EventArgs> UpdateDocumentDisplayRequested;

        /// <summary>
        /// This event is used to signal that it is desired to notify the user of something.
        /// </summary>
        public static event EventHandler<UserNotificationEventArgs> UserNotificationRequested;

        #endregion

        #region DocDefinitionFilePathname
        /// <summary>
        /// For when the user wants to copy the collection of source-code files to some other location,
        /// this directory-path denotes the destination of that copy-operation.
        /// </summary>
        public string DocDefinitionFilePathname
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _docDefinitionFilePathname; }
            set
            {
                if (value != _docDefinitionFilePathname)
                {
                    _docDefinitionFilePathname = value;
                    Notify(  );
                }
            }
        }

        private string _docDefinitionFilePathname;
        #endregion

        #region ProgramTitlebarText
        /// <summary>
        /// Get the text to display in the title-bar of the main window.
        /// </summary>
        public string ProgramTitlebarText
        {
            get
            {
                if (_programTitlebarTitle == null)
                {
                    var app = Application.Current as App;
                    if (app == null)
                    {
                        _programTitlebarTitle = "Vendor ProductName";
                    }
                    else
                    {
                        _programTitlebarTitle = app.VendorName + " " + app.ProductName;
                    }
                }
                return _programTitlebarTitle;
            }
        }
        private string _programTitlebarTitle;
        #endregion

        #region AboutWindowTitlebarText
        /// <summary>
        /// Get the text to display in the title-bar of the About window.
        /// </summary>
        public string AboutWindowTitlebarText
        {
            get
            {
                return ProgramTitlebarText + "  - About this";
            }
        }
        #endregion

        #region OptionsWindowTitlebarText
        /// <summary>
        /// Get the text to display in the title-bar of the Options window.
        /// </summary>
        public string OptionsWindowTitlebarText
        {
            get
            {
                if (_optionsWindowTitlebarText == null)
                {
                    string firstPart = ProgramTitlebarText;
                    _optionsWindowTitlebarText = firstPart + "  - User-Configurable Options";
                }
                return _optionsWindowTitlebarText;
            }
        }
        private string _optionsWindowTitlebarText;
        #endregion

        #region ProgramVersion
        /// <summary>
        /// Get or set the string that denotes the 'program-version' that this program wants to label itself as,
        /// as it would be presented to the end-user.
        /// </summary>
        public string ProgramVersion
        {
            get { return _programVersion; }
            set
            {
                if (value != _programVersion)
                {
                    if (value != null)
                    {
                        _programVersion = value;
                    }
                    else
                    {
                        _programVersion = "ProgramVersion";
                    }
                    Notify( nameof( ProgramVersion ) );
                }
            }
        }
        /// <summary>
        /// This string denotes the 'program-version' that this program wants to label itself as,
        /// as it would be presented to the end-user.
        /// </summary>
        private string _programVersion = "Version";
        #endregion

        #region The
        /// <summary>
        /// Get the (singleton) instance of this view-model.
        /// </summary>
        public static ApplicationViewModel The
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ApplicationViewModel();
                }
                return _instance;
            }
        }
        /// <summary>
        /// This is the one, singleton instance of this class.
        /// </summary>
        private static ApplicationViewModel _instance;
        #endregion

        #region commands

        public ICommand UpdateDocumentDisplayCommand
        {
            get
            {
                if (_updateDocumentDisplayCommand == null)
                {
                    _updateDocumentDisplayCommand = new UiCommand( _ =>
                    {
                        UpdateDocumentDisplayRequested?.Invoke( this, EventArgs.Empty );
                    } );
                }
                return _updateDocumentDisplayCommand;
            }
        }
        private ICommand _updateDocumentDisplayCommand;

        public ICommand EditOptionsCommand
        {
            get
            {
                if (_editOptionsCommand == null)
                {
                    _editOptionsCommand = new UiCommand( _ =>
                    {
                        EditOptionsRequested?.Invoke( this, EventArgs.Empty );
                    } );
                }
                return _editOptionsCommand;
            }
        }
        private ICommand _editOptionsCommand;

        public ICommand ShowHelpAboutMeCommand
        {
            get
            {
                if (_showHelpAboutMeCommand == null)
                {
                    _showHelpAboutMeCommand = new UiCommand( _ =>
                    {
                        ShowHelpAboutMeRequested?.Invoke( this, EventArgs.Empty );
                    } );
                }
                return _showHelpAboutMeCommand;
            }
        }
        private ICommand _showHelpAboutMeCommand;

        public ICommand CreateNewDocCommand
        {
            get
            {
                if (_createNewDocCommand == null)
                {
                    _createNewDocCommand = new UiCommand( _ =>
                    {
                        Debug.WriteLine( "Create new document." );
                        //SelectDocDefinitionFilePathnameRequested?.Invoke( this, EventArgs.Empty );
                    } );
                }
                return _createNewDocCommand;
            }
        }
        private ICommand _createNewDocCommand;

        public ICommand SelectDocDefinitionFilePathnameCommand
        {
            get
            {
                if (_selectDocDefinitionFilePathnameCommand == null)
                {
                    _selectDocDefinitionFilePathnameCommand = new UiCommand( _ =>
                    {
                        SelectDocDefinitionFilePathnameRequested?.Invoke( this, EventArgs.Empty );
                    } );
                }
                return _selectDocDefinitionFilePathnameCommand;
            }
        }
        private ICommand _selectDocDefinitionFilePathnameCommand;

        /// <summary>
        /// The ICommand that causes this program to quit.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new UiCommand( _ => Exit() );
                }
                return _exitCommand;
            }
        }
        private UiCommand _exitCommand;

        #endregion commands

        #region user-notification methods

        public void NotifyUser( string message )
        {
            // Raise the appropriate event, so that the view may respond to it.
            UserNotificationRequested?.Invoke( this, new UserNotificationEventArgs( message: message, isWarning: false, isError: false ) );
        }

        public void NotifyUserOfError( string message )
        {
            // Raise the appropriate event, so that the view may respond to it.
            UserNotificationRequested?.Invoke( this, new UserNotificationEventArgs( message: message, isWarning: false, isError: true ) );
        }

        public void NotifyUserOfMistake( string message )
        {
            // Raise the appropriate event, so that the view may respond to it.
            UserNotificationRequested?.Invoke( this, new UserNotificationEventArgs( message: message, isWarning: true, isError: false, isUserMistake: true ) );
        }

        #endregion user-notification methods

        public void Exit()
        {
            // In Metro, this would be CoreApplication.Exit
            Application.Current.MainWindow.Close();
        }

    }
}
