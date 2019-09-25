using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurst.BaseLib;


namespace AcroGen
{
    public class AcrogenUserSettings : UserSettings
    {
        #region The
        /// <summary>
        /// Get the one instance of the VsDevToolUserSettings class.
        /// </summary>
        public static AcrogenUserSettings The
        {
            get
            {
                if (_userSettings == null)
                {
                    _userSettings = GetInstance<AcrogenUserSettings>( App.The, isToSaveLocation: true, isToSaveSize: true );
                }
                // This extra-step ensures this property is set, even if the settings-object was last deserialized without this value.
                _userSettings.PositionOfMainWindow.IsSavingSize = true;
                return _userSettings;
            }
        }
        /// <summary>
        /// This is the single instance of this class.
        /// </summary>
        private static AcrogenUserSettings _userSettings;
        #endregion

        #region DocDefinitionFilePathname
        /// <summary>
        /// Get or set the file pathname that contains the document-specification that we are currently editing.
        /// </summary>
        public string DocDefinitionFilePathname
        {
            get { return _docDefinitionFilePathname; }
            set
            {
                if (value != _docDefinitionFilePathname)
                {
                    _docDefinitionFilePathname = value;
                    IsChanged = true;
                }
            }
        }
        #endregion

        /// <summary>
        /// This denotes the file pathname that contains the document-specification that we are currently editing.
        /// </summary>
        private string _docDefinitionFilePathname;

    }
}
