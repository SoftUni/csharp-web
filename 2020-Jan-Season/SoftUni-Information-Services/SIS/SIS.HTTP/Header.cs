namespace SIS.HTTP
{
    /// <summary>
    /// Represents an HTTP Header information, consisting of <c>Name</c> and <c>Value</c>.
    /// </summary>
    public class Header
    {
        //------------- CONSTRUCTORS -------------
        /// <summary>
        /// Initializes a new <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        public Header(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        //-------------- PROPERTIES --------------
        /// <summary>
        /// HTTP Header Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// HTTP Header Value.
        /// </summary>
        public string Value { get; set; }

        //------------ PUBLIC METHODS ------------
        /// <summary>
        /// Returns formatted HTTP Header.
        /// </summary>
        /// <returns>Formatted HTTP Header</returns>
        public override string ToString()
        {
            return $"{this.Name}: {this.Value}";
        }
    }
}
