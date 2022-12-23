namespace OpenVMSys.Core.Console.Module.Security;

internal class SecurityKey
{
    private readonly string? _key;
    public string? Key
    {
        get => _key;
        init => _key = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Permission { get; init; }
    public string? Ident { get; init; }
}