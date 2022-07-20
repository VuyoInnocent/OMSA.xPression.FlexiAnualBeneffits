using FileHelpers;

namespace SaGE.Correspondence.Domain
{
    [DelimitedRecord("|")]

    public class DCTM
    {
        public string BusinessArea;
        public string SourceName;
        public string Description;
        public string TypeFlag;
        public string Permission;
        public string Category;
    }
}