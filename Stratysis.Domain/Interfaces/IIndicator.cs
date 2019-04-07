using Stratysis.Domain.Core;

namespace Stratysis.Domain.Interfaces
{
    public interface IIndicator
    {
        void Calculate(Slice slice);
    }
}
