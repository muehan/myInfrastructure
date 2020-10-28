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
        var resourceGroup = new ResourceGroup("myinfrastructure");

        var publicIp = new PublicIp("gameServer1publicId", new PublicIpArgs {
            ResourceGroupName = resourceGroup.Name,
            AllocationMethod = "Static"
        });

        var virtualNetwork = new VirtualNetwork("myInfrastructureNetwork", new VirtualNetworkArgs
        {
            AddressSpaces =
                    {
                        "10.0.0.0/16",
                    },
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
        });

        var vmSubnet = new Subnet("vmSubnet", new SubnetArgs
        {
            ResourceGroupName = resourceGroup.Name,
            VirtualNetworkName = virtualNetwork.Name,
            AddressPrefixes =
            {
                "10.0.2.0/24",
            },
        });

        var gameServerNetworkInterface = new NetworkInterface("gameServer1NetworkInterface", new NetworkInterfaceArgs
        {
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            IpConfigurations =
            {
                new NetworkInterfaceIpConfigurationArgs
                {
                    Name = "internal",
                    SubnetId = vmSubnet.Id,
                    PrivateIpAddressAllocation = "Dynamic",
                    PublicIpAddressId = publicIp.Id,
                },
            },
        });

        var cfg = new Config();
        var secretPassword = cfg.RequireSecret("adminpassword");

        var gameServer1 = new LinuxVirtualMachine("linuxGameServer1", new LinuxVirtualMachineArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Size = "Standard_B2s",
            AdminUsername = "mueha0",
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
}
