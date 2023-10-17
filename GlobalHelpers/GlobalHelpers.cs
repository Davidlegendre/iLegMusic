
namespace Configuration.GlobalHelpers
{
    public class GlobalHelpers : IGlobalHelpers
    {
        public bool IsNullOrWhiteSpaceAndNumber(string? numtexto, bool IsOptional = false)
        { 
            bool result = true;
            if (string.IsNullOrWhiteSpace(numtexto) && !IsOptional) return false;
            if (IsOptional && string.IsNullOrWhiteSpace(numtexto)) return true;
            foreach (var c in numtexto)
            {
                if (!char.IsNumber(c))
                {
                    result = false; break;
                }
            }
            return result;
        }
    }
}
