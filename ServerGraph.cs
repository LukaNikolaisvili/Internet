/*
ASSIGNMENT#1 - COIS-3020

TEAM MEMBERS:

Luka Nikolaisvili - 0674677
Farzad Imran - 0729901
Freddrick Nkwonta - 0703772

-----------------

*/

using System.Linq.Expressions;

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

public class ServerGraph : WebGraph
{
    // 3 marks
    // Webserver Constructor


    private class WebServer
    {
        public string Name;
        public List<WebPage> P;
        public List<bool> E;

        public WebServer(string name)
        {
            Name = name;
            P = new List<WebPage>();
            E = new List<bool>();

        }

    }
    // Server, Connection, and Number of Server Declarations
    private WebServer[] V;
    private bool[,] E;
    private int NumServers;
    public static string firstPage = "";
    public static string secondPage = "";

    //2 marks
    // Create an empty server graph
    // Server Graph Initialization
    public ServerGraph()
    {

        V = new WebServer[1];
        E = new bool[1, 1];
        NumServers = 0;

    }


    // 2 marks
    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        for (int i = 0; i < NumServers; i++)  // look through the different existing servers 
        {
            if (string.Equals(V[i].Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase)) //compare the name giving to the server names 
                return i;

        }
        return -1;
    }

    public int FindServerFromOutside(string name)
    {
        // Call the private FindServer method internally
        return FindServer(name);
    }


    // 3 marks
    // Double the capacity of the server graph with the respect to web servers
    private void DoubleCapacity()
    {

        // Double the capacity of V
        int newVLength = V.Length * 2;
        Array.Resize(ref V, newVLength);


        // Double the capacity of E
        int rowCounter = E.GetLength(0);
        int columnCounter = E.GetLength(1);


        bool[,] doubledMatrix = new bool[rowCounter * 2, columnCounter * 2];


        Array.Copy(E, 0, doubledMatrix, 0, rowCounter * columnCounter);

        E = doubledMatrix;
    }
    // 3 marks
    // Add a server with the given name and connect it to the other server
    // Return true if successful; otherwise return false
    public bool AddServer(string name, string other)
    {



        // Initial If statement to see if there is a server to connect to
        // If none (0) the method would not work because it calls 2 servers to connect to
        // Each other. So it initializes a Root Server.
        if (NumServers == 0)
        {
            Console.WriteLine("No servers Exist");
            Console.WriteLine("Creating new server called 'Root' ");

            V[NumServers] = new WebServer("Root");
            E[0, 0] = false;
            NumServers = NumServers + 1;

            Console.WriteLine("No servers Added ");
            return false;

        }

        //check if name is passed by user is empty 
        if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(other))
        {
            Console.WriteLine("you can not pass empty variable name");
            return false;

        }

        int i, j;

        // This is the other if statement to add servers normally
        if (NumServers >= 1)
        {
            // if the second server passed as a variable exists, the code will continue
            if ((j = FindServer(other)) != -1)
            {
                // Makes sure the server you want to add does not exist
                if ((i = FindServer(name)) == -1)
                {
                    // If all the slots in ServerGraph are taken up,
                    // You need to make more slots to fill up!
                    if (NumServers == V.Length)
                    {
                        DoubleCapacity();
                    }
                    // Creates the Webserver with the passed name:
                    V[NumServers] = new WebServer(name);
                    E[NumServers, j] = true;
                    E[j, NumServers] = true;

                    NumServers++;

                    // Tells the user the action so they know it worked.
                    Console.WriteLine("Added server: " + name);

                    return true;

                }
            }
            // The server the user wanted to connect with doesn't exist, so a server cannot be made!
            else
            {
                Console.WriteLine("Other server doesnt exists.");
                return false;

            }

            return false;


        }

        return false;



    }
    // 3 marks
    // Add a webpage to the server with the given name
    // Return true if successful; otherwise return false
    public bool AddWebPage(WebPage w, string name)
    {
        int i = FindServer(name);  // finds out if server exist 

        if (i > -1)
        {
            V[i].P.Add(w); // adds webpage
            return true;
        }
        return false;

    }
    // 4 marks
    // Remove the server with the given name by assigning its connections
    // and webpages to the other server
    // Return true if successful; otherwise return false
    public bool RemoveServer(string name, string other)
    {
        int i = FindServer(name);
        int j = FindServer(other);
        // Both servers must exist for the Remove function to execute
        if (i == -1 || j == -1)
        {
            Console.WriteLine("Both servers or one of the servers does not exist.");
            return false;
        }
        else
        {

            // Moves each Webpage from named server to other server 
            foreach (WebPage page in V[i].P)
            {
                V[j].P.Add(page);
                Console.WriteLine(page + "is being moved to server" + other);
            }


            // Moves every server to fill up the gap left by the removed server
            // and then move the connections
            for (int s = i; s < NumServers - 1; s++)
            {
                V[s] = V[s + 1];
                for (int t = 0; t < NumServers; t++)
                {
                    E[s, t] = E[s + 1, t];
                    E[t, s] = E[t, s + 1];
                }
            }


            Console.WriteLine("The server with the name '" + name + "' has been removed");

            NumServers--; // reduces server count 

            return true;

        }


    }

    // Remove the webpage from the server with the given name
    // Return true if successful; otherwise return false
    public bool RemoveWebPage(string webpage, string name)
    {

        return false;
    }
    // 3 marks
    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server
    public bool AddConnection(string from, string to)
    {
        //find if both servers exists 
        int indexOfFrom = FindServer(from);
        int indexOfTo = FindServer(to);


        //if both servers create a connection between them 
        if (indexOfFrom != -1 && indexOfTo != -1)
        {
            V[indexOfFrom].E[indexOfTo] = true;
            return true;
        }
        return false;

    }
    // 10 marks
    // Return all servers that would disconnect the server graph into
    // two or more disjoint graphs if ever one of them would go down
    // Hint: Use a variation of the depth-first search
    public string[] CriticalServers()
    {
        int time = 0;                             // An initial time variable utilized for disc[] and low[]
        bool[] visited = new bool[NumServers];    // Sets Servers as visiited
        int[] disc = new int[NumServers];         // Holds the Discovery Time for any server
        int[] low = new int[NumServers];          // Holds the Loest Discovery Time for the server
        int[] parent = new int[NumServers];       // This array holds the parent node
        bool[] artipoints = new bool[NumServers]; // A boolean array that holds articulation points (Critical Servers)
        List<string> result = new List<string>(); // Use a list to store the resulting critical servers

        // For every unvisited server, perform a recursive Deep-First Search function to find critical points
        for (int i = 0; i < NumServers; i++)
        {
            if (!visited[i])
            {
                CriticalServers(i, ref time, visited, disc, low, parent, artipoints, result);
            }
        }

        return result.ToArray(); // Convert the list to an array and return
    }

    private void CriticalServers(int i, ref int time, bool[] visited, int[] disc, int[] low, int[] parent, bool[] artipoints, List<string> result)
    {
        int children = 0;          // There are no initial children at the start of the code until search continues
        disc[i] = low[i] = ++time; // Increases the time variable so the find order can be consistent.
        visited[i] = true;         // Now that we're in the server, set it as visited.

        // Now for every server:
        for (int j = 0; j < NumServers; j++)
        {
            // Check if the current server connects to them, the server hasn't been visited
            // and if that server exists in the first place.
            if (E[i, j] && !visited[j] && V[j] != null)
            {
                children++;     // A child has been found
                parent[j] = i;  // Set the current server as a parent of j
                CriticalServers(j, ref time, visited, disc, low, parent, artipoints, result); // Enter Recursion

                // Check if the server j has any connections to the current server's parents
                // set the current server's low value as the lowest value.
                low[i] = Math.Min(low[i], low[j]);

                // i is a critical server (articulation point) if any of the following statements hold true:

                // Case 1:  i is the root of the tree and has 2 or more children
                // OR
                // Case 2: i isn't a root of the tree, and one of its children have a lowest discovery time 
                // greater than i's discovery time
                if ((parent[i] == -1 && children > 1) || (parent[i] != -1 && low[j] >= disc[i]))
                {
                    artipoints[i] = true;
                }
            }
            // Otherwise update the current server's low values.
            else if (j != parent[i])
            {
                low[i] = Math.Min(low[i], disc[j]);
            }
        }

        if (artipoints[i])
        {
            result.Add(V[i].Name); // Add the critical server to the result list
        }
    }


    // 6 marks
    // Return the shortest path from one server to another
    // Hint: Use a variation of the breadth-first search

    // Perform breadth-first search to find the shortest path from one server to another
    public int ShortestPath(string from, string to)
    {
        int startIndex = FindServer(from); // Find index of the starting server
        int endIndex = FindServer(to);     // Find index of the destination server

        // Check if both servers exist
        if (Startpoint == -1 || endpoint == -1)
        {
            //when it will jump in this if statement it will print -1
            return -1;
        }

        bool[] visited = new bool[NumServers]; // Array for track visited servers
        int[] distances = new int[NumServers]; // Array for store distances from the starting server
        Queue<int> queue = new Queue<int>();   // Queue for BFS traversal

        // Initialize visited array and distances array
        for (int i = 0; i < NumServers; i++)
        {
            visited[i] = false;
            distances[i] = -1; // Initialize distances to -1 (indicating unreachable)
        }

        // Enqueue the starting server index and mark it as visited
        queue.Enqueue(startIndex);
        visited[startIndex] = true;
        distances[startIndex] = 0; // Distance from starting server to itself is 0

        // Perform BFS traversal
        while (queue.Count > 0)
        {
            int currentServerIndex = queue.Dequeue();

            //  returns the distance from the starting server if it reach the server its going to
            if (currentServerIndex == endIndex)
            {
                return distances[endIndex];
            }

            // Explore neighbors of the current server
            for (int neighborIndex = 0; neighborIndex < NumServers; neighborIndex++)
            {
                // Check if the neighbor is connected to the current server and is not visited
                if (E[currentServerIndex, neighborIndex] && !visited[neighborIndex])
                {
                    queue.Enqueue(neighborIndex);     // Enqueue the neighbor
                    visited[neighborIndex] = true;    // Mark neighbor as visited
                    distances[neighborIndex] = distances[currentServerIndex] + 1; // Update distance
                }
            }
        }

        // If no path found, return -1
        return -1;
    }



    // Method to print out the critical servers, using each string in the found array.
    public void PrintCriticalServers(string[] criticalServers)
    {
        Console.WriteLine("Critical Servers:");
        foreach (string server in criticalServers)
        {
            Console.WriteLine(server);
        }
    }



    // 4 marks
    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public new void PrintGraph()
    {
        // For every Server:
        for (int i = 0; i < NumServers; i++)
        {
            //Write the Server's name
            Console.WriteLine("Server: " + V[i].Name);
            Console.WriteLine("Connections:");

            // And every connection the server has.
            for (int j = 0; j < NumServers; j++)
            {
                if (E[i, j])
                {
                    Console.WriteLine("- " + V[j].Name);
                }
            }

            // And then print each Webpage stored on the server
            Console.WriteLine("Webpages:");
            for (int s = 0; s < V[i].P.Count; s++)
            {
                Console.WriteLine("- " + V[i].P[s].Name);
            }

            Console.WriteLine();
        }
    }



    // Main method
    static void Main()
    {
        // Initialization
        ServerGraph serverGraph = new ServerGraph();
        WebGraph webGraph = new WebGraph();
        // Initial function call for AddServer to create a Root server
        // This way the user does not need to enter the add server command twice
        // To make the server they wanted at program launch.
        serverGraph.AddServer("", "");

        while (true)
        {

            // Basic Interface
            Console.WriteLine("\n1: Add Server ");
            Console.WriteLine("2: Add Page ");
            Console.WriteLine("3: Find shortest Path");
            Console.WriteLine("4: double the capacity ");
            Console.WriteLine("5: Print the graph ");
            Console.WriteLine("6: Locate the Critical servers ");
            Console.WriteLine("7: Remove the page ");
            Console.WriteLine("8: Remove the Server ");
            Console.WriteLine("9: Add Hyperlink between pages ");
            Console.WriteLine("0: Find AVG shortest Path");
            Console.WriteLine("10: Remove Link");
            Console.WriteLine("exit: type [exit] to close the application ");
            // Creates a fancy bar separator so commands are away from prompts
            for (int i = 0; i < 36; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine("\ntype the opertaion you would like to perform ");
            string input = Console.ReadLine();

            // AddServer
            if (input == "1")
            {
                if (serverGraph.NumServers > 0)
                {
                    Console.WriteLine("Enter your server name: ");
                    string server1 = Console.ReadLine();
                    Console.WriteLine("What server do you want to connect it to?: ");
                    string server2 = Console.ReadLine();
                    Console.WriteLine("\nResult:");
                    serverGraph.AddServer(server1, server2);
                }
            }

            // Add Webpage
            else if (input == "2")
            {
                Console.WriteLine("Enter your website name: ");
                string websiteName = Console.ReadLine();
                Console.WriteLine("Enter the server you want to host your webiste at: ");
                string hostingServer = Console.ReadLine();
                webGraph.AddPage(websiteName, hostingServer, serverGraph);

            }

            // The Average Shortest Path
            else if (input == "3")
            {
                Console.WriteLine("Enter starting server: ");
                string startingServer = Console.ReadLine();
                Console.WriteLine("Enter the server you want to find the shortest path to: ");
                string serverToGetTo = Console.ReadLine();
                Console.WriteLine("\nResult: ");
                Console.WriteLine(serverGraph.ShortestPath(startingServer, serverToGetTo));

            }

            // Doubles the Server's Capacity
            else if (input == "4")
            {

                int oldCapacity = serverGraph.V.Length;
                serverGraph.DoubleCapacity();
                int newCapacity = serverGraph.V.Length;

                Console.WriteLine("server capacity incresed from " + oldCapacity + " to " + newCapacity);
            }

            // Prints the Graph
            else if (input == "5")
            {


                Console.WriteLine("Printing graph...\n");
                serverGraph.PrintGraph();
                webGraph.PrintGraph();

            }

            // Lists all critical servers for the user
            else if (input == "6")
            {
                string[] criticalServers = serverGraph.CriticalServers();
                serverGraph.PrintCriticalServers(criticalServers);
            }

            // Remove a webpage from a server
            else if (input == "7")
            {
                Console.WriteLine("Enter the name of the page you want to remove ");
                string pageName = Console.ReadLine();
                webGraph.RemovePage(pageName, serverGraph);
            }

            // Removes a server
            else if (input == "8")
            {
                Console.WriteLine("Enter the name of the server you wish to remove : ");
                string firstServerName = Console.ReadLine();
                Console.WriteLine("Enter the server you want to move '" + firstServerName + "'s' connections to: ");
                string secondServerName = Console.ReadLine();
                serverGraph.RemoveServer(firstServerName, secondServerName);
            }

            // Adds a link to a given webpage.
            else if (input == "9")
            {
                Console.WriteLine("Enter the 1st page name ");
                firstPage = Console.ReadLine();
                Console.WriteLine("Enter the 2nd page name ");
                secondPage = Console.ReadLine();
                webGraph.AddLink(firstPage, secondPage);


            }

            else if (input == "0")
            {

                Console.WriteLine("Enter the name of webpage ");
                string webpage = Console.ReadLine();
                webGraph.AvgShortestPaths(webpage, serverGraph);
            }

            else if (input == "10")
            {
                Console.WriteLine("Enter page name that you want to unlink from ");
                string pageFrom = Console.ReadLine();
                Console.WriteLine("Enter page name that you want to be unlinked to ");
                string pageTo = Console.ReadLine();
                webGraph.RemoveLink(pageFrom, pageTo);
            }

            // Exits the While Loop
            else if (input == "exit")
            {

                Environment.Exit(0);

            }

            // If the user tries something funny and doesn't enter a valid command
            // Close but no cigar!
            else
            {
                Console.WriteLine("'Oops you were close bud, try again!'");

            }

        }
    }
}

