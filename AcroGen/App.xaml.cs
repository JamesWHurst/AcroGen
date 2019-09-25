using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hurst.DFBaseLibWpf;


namespace AcroGen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : DfDesktopApplication
    {
        #region ProductName
        /// <summary>
        /// Get the name of this application for display within titlebars and the like,
        /// which in this case is "VS-Dev-Tool".
        /// </summary>
        public override string ProductName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return ApplicName; }
        }
        #endregion

        #region The
        /// <summary>
        /// Get the App object that is our application
        /// </summary>
        public new static App The
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return Application.Current as App; }
        }
        #endregion

        #region VendorName
        /// <summary>
        /// Get the (short, one-word version of) name of the maker or owner of this software,
        /// as would be used for the first part of the CommonDataPath, and the window title-bar prefix.
        /// This is "GTI" for this application.
        /// </summary>
        public override string VendorName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return "GTI";
            }
        }
        #endregion

        #region OnStartup
        /// <summary>
        /// Override the OnStartup method to setting some shit.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup( StartupEventArgs e )
        {
            // Ensure there is only one instance of this application running at any one time.
            _singleInstanceEvent = new EventWaitHandle( false, EventResetMode.AutoReset, _eventName, out bool wasCreated );
            if (!wasCreated)
            {
                // If the event-handle was not newly created, then that means another instance of this program is already running.
                _singleInstanceEvent.Set();
                Shutdown();
            }
            else // this is the first instance of this program.
            {
                // Create a new Task that waits for that event to be signalled (meaning a second instance of this program tried to launch)
                // and, when that happens, make *this* instance become active and visible to the user.
                SynchronizationContext ctx = SynchronizationContext.Current;
                Task.Factory.StartNew( () =>
                {
                    while (true)
                    {
                        _singleInstanceEvent.WaitOne();
                        ctx.Post( _ => MakeActiveApplication(), null );
                    }
                } );
            }

            // Configure the logging.
            //CBL           LogManager.Config.SubjectProgramVersion = Util.GetVersion();

            base.OnStartup( e );
        }
        #endregion

        #region MakeActiveApplication
        /// <summary>
        /// Make this program the top-most (ie the "active") one.
        /// </summary>
        private void MakeActiveApplication()
        {
            if (MainWindow != null)
            {
                MainWindow.Activate();
                // These next 2 statements are to bring this program-window to the foreground,
                // without making it always insist upon staying in the foreground (which would be annoying).
                MainWindow.Topmost = true;
                MainWindow.Topmost = false;
                MainWindow.Focus();
            }
        }
        #endregion

        /// <summary>
        /// The purpose of this string-literal is just to provide one source for this text value.
        /// </summary>
        public const string ApplicName = "AcroGen";
        private const string _eventName = ApplicName;
        /// <summary>
        /// This is used for enforcing the running of only one copy of this program.
        /// </summary>
        private EventWaitHandle _singleInstanceEvent;
    }
}
