namespace CTM.TestAutomation.Core.ExtensionHelpers
{
    using System;

    /// <summary>
    /// The string value helper to allow string values to be
    /// stored with enums
    /// </summary>
    public class EnumStringValue : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumStringValue"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public EnumStringValue(string value)
        {
            this.StringValue = value;
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue { get; protected set; }
    }
}