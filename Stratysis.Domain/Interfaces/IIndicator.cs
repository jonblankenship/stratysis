using Stratysis.Domain.Core;

namespace Stratysis.Domain.Interfaces
{
    /// <summary>
    /// Interface representing an indicator
    /// </summary>
    public interface IIndicator
    {
        /// <summary>
        /// Calculates the indicator values for all securities in the <see cref="slice"/>
        /// </summary>
        /// <param name="slice">The current <see cref="Slice"/></param>
        void Calculate(Slice slice);

        /// <summary>
        /// Returns a <see cref="bool"/> indicating whether this indicator is warmed up for the given <see cref="security"/> and <see cref="periodOffset"/>
        /// </summary>
        /// <param name="security">The symbol of the security</param>
        /// <param name="periodOffset">The period offset</param>
        /// <returns></returns>
        bool IsWarmedUp(string security, int periodOffset);
    }
}
