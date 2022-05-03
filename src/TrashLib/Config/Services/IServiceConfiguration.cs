namespace TrashLib.Config.Services;

public interface IServiceConfiguration
{
    string Name { get; set; }
    string BaseUrl { get; }
    string ApiKey { get; }
}
