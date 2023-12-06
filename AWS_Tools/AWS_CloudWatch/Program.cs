using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;

namespace CFNListResources
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create the CloudFormation client
            var cfnClient = new AmazonCloudFormationClient();

            // List the resources for each stack
            await ListResources(cfnClient,await cfnClient.DescribeStacksAsync());
        }

        //
        // Method to list stack resources and other information
        private static async Task ListResources(
          IAmazonCloudFormation cfnClient,DescribeStacksResponse responseDescribeStacks)
        {
            Console.WriteLine("Getting CloudFormation stack information...");

            foreach(Stack stack in responseDescribeStacks.Stacks)
            {
                // Basic information for each stack
                Console.WriteLine("\n------------------------------------------------");
                Console.WriteLine($"\nStack: {stack.StackName}");
                Console.WriteLine($"  Status: {stack.StackStatus.Value}");
                Console.WriteLine($"  Created: {stack.CreationTime}");

                // The tags of each stack (etc.)
                if(stack.Tags.Count>0)
                {
                    Console.WriteLine("  Tags:");
                    foreach(Tag tag in stack.Tags)
                        Console.WriteLine($"    {tag.Key}, {tag.Value}");
                }

                // The resources of each stack
                DescribeStackResourcesResponse responseDescribeResources =
                  await cfnClient.DescribeStackResourcesAsync(new DescribeStackResourcesRequest
                  {
                      StackName=stack.StackName
                  });
                if(responseDescribeResources.StackResources.Count>0)
                {
                    Console.WriteLine("  Resources:");
                    foreach(StackResource resource in responseDescribeResources.StackResources)
                        Console.WriteLine($"    {resource.LogicalResourceId}: {resource.ResourceStatus}");
                }
            }
            Console.WriteLine("\n------------------------------------------------");
        }
    }
}
