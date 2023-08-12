namespace DriverRater.Api.Plumbing.KeyVault;

using System.ComponentModel.DataAnnotations;

public class KeyVaultOptions
{
    public static string Key => "KeyVault";
    [Required]   
    public string StorageEncryptionKeyName { get; set; }
    [Required]
    public string KeyVaultName { get; set; }
}