﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5483
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 

namespace SaGE.Correspondence.Domain
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DCTMMeta
    {

        private string pDFField;

        private string lOBField;

        private string busAreaField;

        private string docPackCodeField;

        private string processField;

        private string numFlagField;

        private string numberField;

        private string dateField;

        private string formatField;

        private string permissionField;

        private string categoryField;

        private string descriptionField;

        /// <remarks/>
        public string PDF
        {
            get { return this.pDFField; }
            set { this.pDFField = value; }
        }

        /// <remarks/>
        public string LOB
        {
            get { return this.lOBField; }
            set { this.lOBField = value; }
        }

        /// <remarks/>
        public string BusArea
        {
            get { return this.busAreaField; }
            set { this.busAreaField = value; }
        }

        /// <remarks/>
        public string DocPackCode
        {
            get { return this.docPackCodeField; }
            set { this.docPackCodeField = value; }
        }

        /// <remarks/>
        public string Process
        {
            get { return this.processField; }
            set { this.processField = value; }
        }

        /// <remarks/>
        public string NumFlag
        {
            get { return this.numFlagField; }
            set { this.numFlagField = value; }
        }

        /// <remarks/>
        public string Number
        {
            get { return this.numberField; }
            set { this.numberField = value; }
        }

        /// <remarks/>
        public string Date
        {
            get { return this.dateField; }
            set { this.dateField = value; }
        }

        /// <remarks/>
        public string Format
        {
            get { return this.formatField; }
            set { this.formatField = value; }
        }

        /// <remarks/>
        public string Permission
        {
            get { return this.permissionField; }
            set { this.permissionField = value; }
        }

        /// <remarks/>
        public string Category
        {
            get { return this.categoryField; }
            set { this.categoryField = value; }
        }

        /// <remarks/>
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }
    }
}