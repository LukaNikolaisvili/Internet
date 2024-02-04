/*
ASSIGNMENT#1 - COIS-3020

TEAM MEMBERS:

Luka Nikolaisvili - 0674677
Farzad Imran - 0729901
Freddrick Nkwonta - 0703772

-----------------
*/


using System;
using System.Collections.Generic;

public class WebPage
{
    public string Name { get; set; }
    public string Server { get; set; }
    public List<WebPage> E { get; set; }

    public WebPage(string name, string host)
    {
        Name = name;
        Server = host;
        Console.WriteLine(name + " is hosted on " + host);
        E = new List<WebPage>();

    }

    public int FindLink(string name)
    {
        return 1;
    }
}

public class WebGraph
{
    private List<WebPage> P;

    public WebGraph()
    {
        P = new List<WebPage>();
    }

    // 2 marks
    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {

        for (int i = 0; i < P.Count; i++)
        {
            if (P[i].Name == name)
                return 1;
        }
        return -1;
    }

    // Adds a webpage with the given name, attached to the host server, 
    // and passed with a servergaph object to work with
    public bool AddPage(string name, string host, ServerGraph S)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(host) || S == null)
        {
            return false;
        }
        else
        {
            // Create a new WebPage instance
            WebPage createPage = new WebPage(name, host);

            // Add the webpage to the list of web pages in WebGraph
            P.Add(createPage);

            // Add the webpage to the server graph
            S.P.Add(createPage);

            return true;
        }
    }

    // Removes a page with the given name, with a passed servergraph object to interact with.
    public bool RemovePage(string name, ServerGraph S)
    {

        int id = FindPage(name);
        if (id != -1)
        {
            Console.WriteLine("Page was found and its removed: ");
            S.P.RemoveAt(id);
            return true;
        }
        Console.WriteLine("Page with that name is not found, try again. ");
        return false;
    }

    // Adds a link that connects one webpage to another
    // It's like a server connection but for webpages! :D
    public bool AddLink(string from, string to)
    {

        int indexFrom = FindPage(from);
        int indexTo = FindPage(to);

        if (indexFrom != -1 && indexTo != -1)
        {
            P[indexFrom].E.Add(P[indexTo]);

            return true;
        }

        return false;

    }

    // Removes a link from the starting page that connects to the ending page if found
    public bool RemoveLink(string from, string to)
    {

        int indexFrom = FindPage(from);
        int indexTo = FindPage(to);

        if (indexFrom != -1 && indexTo != 1)
        {


            if (P[indexFrom].E.Contains(P[indexTo]))
            {

                P[indexFrom].E.Remove(P[indexTo]);

                return true;
            }
        }

        return false;

    }

    // 6 marks
    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph

    // public int ShortestPath(string from, string to)
    // {
    //     int Startpoint = FindServer(from);
    //     int endpoint = FindServer(to);

    //     if (Startpoint == -1 || endpoint == -1)
    //     {
    //         Console.WriteLine("Server or server can not found.");
    //         return -1;
    //     }

    //     Queue<int>  Q = new Queue<int>(); 
    //     bool[] visited = new bool[NumServers];
    //     int[] distances = new int[NumServers];

    //     Q.Enqueue(Startpoint); 
    //     visited[Startpoint] = true;
    //     distances[Startpoint] = 0;

    //     while (Q.Count > 0)
    //     {
    //         int currentServerIndex = Q.Dequeue(); 

    //         if (currentServerIndex == endpoint) 
    //             return distances[endpoint]; 
    //         for (int i = 0; i < NumServers; i++)
    //         {
    //             if (E[currentServerIndex, i] && !visited[i]) 
    //             {
    //                 Q.Enqueue(i); 
    //                 visited[i] = true;
    //                 distances[i] = distances[currentServerIndex] + 1; 
    //             }
    //         }
    //     }

    //     Console.WriteLine("No path found.");
    //     return -1;
    // }

    // Prints the webgraph!
    public void PrintGraph()
    {


    }
}


