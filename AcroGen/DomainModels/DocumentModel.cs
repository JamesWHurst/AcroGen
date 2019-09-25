using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AcroGen.DomainModels
{
    public class DocumentModel
    {
        public DocumentModel()
        {

        }

        public string DocumentName { get; set; }

        public float MarginLeft { get; set; }

        public float MarginTop { get; set; }

        public float MarginRight { get; set; }

        public float MarginBottom { get; set; }

        public IList<FieldTextLabel> Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new List<FieldTextLabel>();
                }
                return _labels;
            }
            set { _labels = value; }
        }
        private IList<FieldTextLabel> _labels;

    }
}
