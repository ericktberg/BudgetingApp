using System.Windows.Markup;

namespace Sunsets.Application.MVVM
{
    /// <summary>
    /// Represents a case for a switch converter.
    /// </summary>
    [ContentProperty("Then")]
    public class SwitchConverterCase
    {
        // case instances.
        string _when;
        object _then;

        #region Public Properties.
        /// <summary>
        /// Gets or sets the condition of the case.
        /// </summary>
        public string When { get { return _when; } set { _when = value; } }
        /// <summary>
        /// Gets or sets the results of this case when run through a <see cref="SwitchConverter"/>
        /// </summary>
        public object Then { get { return _then; } set { _then = value; } }
        #endregion
        #region Construction.
        /// <summary>
        /// Switches the converter.
        /// </summary>
        public SwitchConverterCase()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchConverterCase"/> class.
        /// </summary>
        /// <param name="when">The condition of the case.</param>
        /// <param name="then">The results of this case when run through a <see cref="SwitchConverter"/>.</param>
        public SwitchConverterCase(string when, object then)
        {
            // Hook up the instances.
            this._then = then;
            this._when = when;
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("When={0}; Then={1}", When.ToString(), Then.ToString());
        }
    }
}
