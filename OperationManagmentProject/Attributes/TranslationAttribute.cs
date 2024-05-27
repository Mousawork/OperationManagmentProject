namespace OperationManagmentProject.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class TranslationAttribute : Attribute
    {
        public string Arabic { get; }

        public TranslationAttribute(string arabic)
        {
            Arabic = arabic;
        }
    }
}
