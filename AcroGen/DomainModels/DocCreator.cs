using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AcroGen.DomainModels
{
    public class DocCreator
    {
        public void Save(string path)
        {
            Debug.WriteLine("DocCreator.Save(" + path + ")");
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }


        }
    }
}
