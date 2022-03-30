using Pulumi;
using Pulumi.Azure.Compute;
using Pulumi.Azure.Compute.Inputs;
using Pulumi.Azure.Core;
using Pulumi.Azure.Network;
using Pulumi.Azure.Network.Inputs;

class MyStack : Stack
{
    public MyStack()
    {
        // Create an Azure Resource Group
        var resourceGroupSwitzerland = new ResourceGroup(
            "pulumi-switzerland",
            new ResourceGroupArgs
            {
                Location = "SwitzerlandNorth"
            });

        var virtualNetworkswitzerland = CreateVirtualNetwork(
           resourceGroupSwitzerland,
           "pulumiNetworkSwitzerland",
           "10.0.0.0/16");

        var subNetSwitzerland = CreateSubnet(
            "pulumiSubnetSwitzerland",
            resourceGroupSwitzerland,
            virtualNetworkswitzerland,
            "10.0.2.0/24");

        CreateGameServer(
            resourceGroupSwitzerland,
            subNetSwitzerland);

        var resourceGroupUsaWest = new ResourceGroup(
            "pulumi-westus",
            new ResourceGroupArgs
            {
                Location = "westus"
            });

        var virtualNetworkUsa = CreateVirtualNetwork(
           resourceGroupUsaWest,
           "pulumiNetworkWestus",
           "10.0.0.0/16");

        var subNetUsa = CreateSubnet(
            "pulumiSubnetWestus",
            resourceGroupUsaWest,
            virtualNetworkUsa,
            "10.0.2.0/24");

        CreateDebianVpn(
            resourceGroupUsaWest,
            subNetUsa);
    }

    private void CreateDebianVpn(
       ResourceGroup resourceGroup,
       Subnet subnet)
    {
        PublicIp publicIp = CreatePublicIp(
            resourceGroup,
            "debianVpnPublicIp",
            "Static");

        var networkInterface = CreateNetworkInterface(
            resourceGroup,
            subnet,
            publicIp,
            "debianVpnNetworkInterface");

        var cfg = new Config();
        var secretPassword = cfg.RequireSecret("adminpassword");

        var server = new LinuxVirtualMachine(
            "debianVpnUsa",
            new LinuxVirtualMachineArgs
            {
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                Size = "Standard_B2s",
                AdminUsername = "mueha0",
                //PlatformFaultDomain = 3,
                NetworkInterfaceIds =
                {
                    networkInterface.Id,
                },
                AdminPassword = secretPassword,
                DisablePasswordAuthentication = false,
                OsDisk = new LinuxVirtualMachineOsDiskArgs
                {
                    Caching = "ReadWrite",
                    StorageAccountType = "Standard_LRS",
                    DiskSizeGb = 60,
                },
                SourceImageReference = new LinuxVirtualMachineSourceImageReferenceArgs
                {
                    Publisher = "Debian",
                    Offer = "debian-10",
                    Sku = "10",
                    Version = "latest",
                },
            });
    }

    private void CreateGameServer(
       ResourceGroup resourceGroup,
       Subnet subnet)
    {
        var publicIp = CreatePublicIp(
            resourceGroup,
            "gameServer1publicId",
            "Static");

        var gameServerNetworkInterface = CreateNetworkInterface(
            resourceGroup,
            subnet,
            publicIp,
            "gameServer1NetworkInterface");

        var cfg = new Config();
        var secretPassword = cfg.RequireSecret("adminpassword");

        var gameServer1 = new LinuxVirtualMachine(
            "linuxGameServer1",
            new LinuxVirtualMachineArgs
            {
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                Size = "Standard_B2s",
                AdminUsername = "mueha0",
                //PlatformFaultDomain = 3,
                NetworkInterfaceIds =
                {
                    gameServerNetworkInterface.Id,
                },
                AdminPassword = secretPassword,
                DisablePasswordAuthentication = false,
                OsDisk = new LinuxVirtualMachineOsDiskArgs
                {
                    Caching = "ReadWrite",
                    StorageAccountType = "Standard_LRS",
                    DiskSizeGb = 60,
                },
                SourceImageReference = new LinuxVirtualMachineSourceImageReferenceArgs
                {
                    Publisher = "Debian",
                    Offer = "debian-10",
                    Sku = "10",
                    Version = "latest",
                },
            });
    }

    private NetworkInterface CreateNetworkInterface(
        ResourceGroup resourceGroup,
        Subnet subnet,
        PublicIp publicIp,
        string name)
    {
        return new NetworkInterface(
            name,
            new NetworkInterfaceArgs
            {
                Location = resourceGroup.Location,
                ResourceGroupName = resourceGroup.Name,
                IpConfigurations =
                {
                    new NetworkInterfaceIpConfigurationArgs
                    {
                        Name = "internal",
                        SubnetId = subnet.Id,
                        PrivateIpAddressAllocation = "Dynamic",
                        PublicIpAddressId = publicIp.Id,
                    },
                },
            });
    }

    private Subnet CreateSubnet(
        string name,
        ResourceGroup resourceGroup,
        VirtualNetwork virtualNetwork,
        string addressPrefixes)
    {
        return new Subnet(
            name,
            new SubnetArgs
            {
                ResourceGroupName = resourceGroup.Name,
                VirtualNetworkName = virtualNetwork.Name,
                AddressPrefixes = { addressPrefixes },
            });
    }

    private VirtualNetwork CreateVirtualNetwork(
        ResourceGroup resourceGroup,
        string name,
        string addressSpace)
    {
        return new VirtualNetwork(name, new VirtualNetworkArgs
        {
            AddressSpaces =
                    {
                        addressSpace,
                    },
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
        });
    }

    private PublicIp CreatePublicIp(
        ResourceGroup resourceGroup,
        string name,
        string method)
    {
        return new PublicIp(
            name,
            new PublicIpArgs
            {
                ResourceGroupName = resourceGroup.Name,
                AllocationMethod = method,
                Location = resourceGroup.Location,
            });
    }
}
