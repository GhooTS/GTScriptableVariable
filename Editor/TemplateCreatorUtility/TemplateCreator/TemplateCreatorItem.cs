namespace GTVariable.Editor.Utility
{
    public class TemplateCreatorItem
    {
        public FileTemplate Template { get; set; }
        public bool FileExist { get; set; }
        public bool Modify { get; set; }
        public bool Required { get; set; }
        public int DependenceIndex { get; set; }
        public bool Dependence { get; set; } = false;
    }
}