using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging;

namespace AssemblyMiner.Services.Version;

internal class VersionMiner(ILogger<VersionMiner> logger) : IVersionMiner
{
    public string GetInformationalVersion(string assemblyPath)
    {
        logger.LogInformation("Starting to extract InformationalVersion from: {Path}", assemblyPath);

        using var stream = File.OpenRead(assemblyPath);
        logger.LogDebug("Opened file stream for: {Path}", assemblyPath);

        using var peReader = new PEReader(stream);

        if (!peReader.HasMetadata)
        {
            logger.LogError("Assembly {Path} does not contain metadata.", assemblyPath);
            throw new InvalidOperationException("File has no metadata.");
        }

        logger.LogDebug("PE file contains metadata. Proceeding to read metadata.");
        var metadataReader = peReader.GetMetadataReader();

        foreach (var handle in metadataReader.CustomAttributes)
        {
            var attribute = metadataReader.GetCustomAttribute(handle);

            // Check if it's applied to the assembly itself
            if (attribute.Parent.Kind != HandleKind.AssemblyDefinition)
                continue;

            logger.LogDebug("Found custom attribute applied to AssemblyDefinition.");

            var attrTypeName = GetAttributeTypeName(metadataReader, attribute.Constructor);
            logger.LogDebug("Resolved attribute type name: {AttributeType}", attrTypeName);

            if (attrTypeName == "System.Reflection.AssemblyInformationalVersionAttribute")
            {
                logger.LogDebug("Match found for AssemblyInformationalVersionAttribute. Reading value...");

                var valueReader = metadataReader.GetBlobReader(attribute.Value);
                valueReader.ReadUInt16(); // Prolog 0x0001
                var version = valueReader.ReadSerializedString();

                logger.LogInformation("Successfully extracted InformationalVersion: {Version}", version);
                return version;
            }
        }

        logger.LogWarning("AssemblyInformationalVersionAttribute not found in: {Path}", assemblyPath);
        return null;
    }

    private string GetAttributeTypeName(MetadataReader reader, EntityHandle constructorHandle)
    {
        logger.LogDebug("Resolving attribute constructor handle of kind: {Kind}", constructorHandle.Kind);

        switch (constructorHandle.Kind)
        {
            case HandleKind.MemberReference:
                var memberRef = reader.GetMemberReference((MemberReferenceHandle)constructorHandle);
                var container = memberRef.Parent;

                logger.LogDebug("Attribute constructor is a MemberReference. Resolving parent...");

                if (container.Kind == HandleKind.TypeReference)
                {
                    var typeRef = reader.GetTypeReference((TypeReferenceHandle)container);
                    var ns = reader.GetString(typeRef.Namespace);
                    var name = reader.GetString(typeRef.Name);

                    logger.LogDebug("Resolved attribute type: {Namespace}.{Name}", ns, name);
                    return $"{ns}.{name}";
                }
                break;

            case HandleKind.MethodDefinition:
                logger.LogDebug("Attribute constructor is a MethodDefinition. Not expected for assembly-level attributes.");
                break;
        }

        logger.LogWarning("Failed to resolve attribute type name from constructor handle.");
        
        return null;
    }
}
