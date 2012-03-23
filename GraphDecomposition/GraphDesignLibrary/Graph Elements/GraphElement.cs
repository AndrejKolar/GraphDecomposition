
namespace GraphDesignLibrary
{
    public abstract class GraphElement
    {
        private string name;
        private string objectName;

        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
