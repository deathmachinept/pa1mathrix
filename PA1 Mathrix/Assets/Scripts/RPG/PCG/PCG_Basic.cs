using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;


public class PCG_Basic : PCG{

    public int layout_type; // Layout type
    public int room_type; // Room type
    public int room_max; // Max room size
    public int room_min; // Min room size
    public int room_num; // Number of rooms
    private int room_base;
    private int room_radix;
    private bool room_blocked = false; // If room is blocked
    int tenativas = 3500; // limite de tentativa de meter quartos
    ArrayList rooms; // Room arraylist
    int corridor_num;
    int corridor_weight;
    int turning_weight;
    private int roomMax;
    private System.Random R;
    private int count = 0;
    // *A Star
    BinaryHeap open_list;
    BinaryHeap closed_list;
    private bool buildMetro;





    public void updateParam(int g_width, int g_height, int r_type, int r_min, int r_max, int c_num, int c_weight, int t_weight, int room_total)
    {
        R = new System.Random();
        buildMetro = false;
        base.updateParam(g_width, g_height);

        if (g_height == 0)
            g_height = 1;
        if (r_type == 0)
            r_type = 1;
        if (r_min == 0)
            r_min = 1;
        if (r_max == 0)
            r_max = 1;
        if (c_num == 0)
            c_num = 1;
        if (c_weight == 0)
            c_weight = 1;
        roomMax = room_total;

        room_type = r_type; // Room type
        room_min = r_min; // Default 9
        room_max = r_max; // Default 16
        room_base = (int)((room_min + 1) * 0.5);
        room_radix = (int)((room_max - room_min) * 0.5 + 2);




        switch (room_type)

        {
                
            case 0: room_num = (pcgrid_width * pcgrid_height) / (int)(R.Next(room_min, room_max) * room_max) + 1;
                break; // Scattered
            case 1: room_num = (pcgrid_width * pcgrid_height) / (R.Next(room_min, room_max) * room_max * 2) + 1;
                break; // Sparse
            case 2: room_num = (pcgrid_width * pcgrid_height) / (int)(R.Next(room_min, room_max) * room_min * 0.5) + 1;
                break; // Dense
            default: room_num = (pcgrid_width * pcgrid_height) / (R.Next(room_min, room_max) * room_max) + 1;
                break; // Scattered

            //case 0: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max) + 1;
            //    break; // Scattered
            //case 1: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max * 2) + 1;
            //    break; // Sparse
            //case 2: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_min * 0.5) + 1;
            //    break; // Dense
            //default: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max) + 1;
            //    break; // Scattered
        }

        corridor_num = c_num;
        corridor_weight = c_weight;
        turning_weight = t_weight;
    }

    public void generatePCGBasic(byte[,] g, byte[,] objectsDir)
    {


        base.generatePCG(g, objectsDir); // Init grid 
        initRooms(); // Initialize rooms
        initCorridors(); // Initialize corridors
    }

    public void insertCell()
    {
        int posY = 10; // apenas alguma celulas abaixo do limite top
        int posX = (pcgrid_width / 4);
        Vector2 coordenadaDeQuarto = new Vector2(posX, posY);

        Room Cela = new Room(pcgrid_width, pcgrid_height, 0, 0, 1, R, 3, 4, 5, coordenadaDeQuarto);
        rooms.Add(Cela);

        for (int j = Cela.room_y1; j <= Cela.room_y2; j++)
        {
            for (int i = Cela.room_x1; i <= Cela.room_x2; i++)
            {
                pcgrid[i, j] = 1;
                //Debug.Log("Contando cells" + i) ;
            }
        }

        for (int i = Cela.wall_x1; i <= Cela.wall_x2; i++)
        {
            if (pcgrid[i, Cela.wall_y1] != 1) //wall de baixo
            {
                pcgrid[i, Cela.wall_y1] = 2;
                guardarDir[i, Cela.wall_y1] = 4;
            }
            if (pcgrid[i, Cela.wall_y2] != 1)
            {
                pcgrid[i, Cela.wall_y2] = 2;
                guardarDir[i, Cela.wall_y2] = 6;

            }
        }

        for (int j = Cela.wall_y1; j <= Cela.wall_y2; j++)
        {
            if (pcgrid[Cela.wall_x1, j] != 1) //side walls west
            {
                pcgrid[Cela.wall_x1, j] = 2;
                guardarDir[Cela.wall_x1, j] = 7;
            }
            if (pcgrid[Cela.wall_x2, j] != 1)
            {
                pcgrid[Cela.wall_x2, j] = 2;
                guardarDir[Cela.wall_x2, j] = 5;
            }
        }
        // Place openings

            if (pcgrid[Cela.opening[0, 0], Cela.opening[0, 1]] != 1)

            {
                pcgrid[Cela.opening[0, 0], Cela.opening[0, 1]] = 3;
                guardarDir[Cela.opening[0, 0], Cela.opening[0, 1]] = (byte)Cela.opening[0, 2];

            }
        

    }

    public void insertMetro()
    {
        int posY = pcgrid_height - 14;
        int posX = (pcgrid_width / 2);
        Vector2 coordenadaDeQuarto = new Vector2(posX, posY);

        Room Metro = new Room(pcgrid_width, pcgrid_height, 0, 0, 1, R, 1, 30,14, coordenadaDeQuarto);
        rooms.Add(Metro);

        for (int j = Metro.room_y1; j <= Metro.room_y2; j++)
        {
            for (int i = Metro.room_x1; i <= Metro.room_x2; i++)
            {
                pcgrid[i, j] = 1;
                //Debug.Log("Contando cells" + i) ;
            }
        }

        for (int i = Metro.wall_x1; i <= Metro.wall_x2; i++)
        {
            if (pcgrid[i, Metro.wall_y1] != 1) //wall de baixo
            {
                pcgrid[i, Metro.wall_y1] = 2;
                guardarDir[i, Metro.wall_y1] = 4;
            }
            if (pcgrid[i, Metro.wall_y2] != 1)
            {
                pcgrid[i, Metro.wall_y2] = 2;
                guardarDir[i, Metro.wall_y2] = 6;

            }
        }

        for (int j = Metro.wall_y1; j <= Metro.wall_y2; j++)
        {
            if (pcgrid[Metro.wall_x1, j] != 1) //side walls west
            {
                pcgrid[Metro.wall_x1, j] = 2;
                guardarDir[Metro.wall_x1, j] = 7;
            }
            if (pcgrid[Metro.wall_x2, j] != 1)
            {
                pcgrid[Metro.wall_x2, j] = 2;
                guardarDir[Metro.wall_x2, j] = 5;
            }
        }


        // Place openings
        for (int k = 0; k < Metro.opening_num; k++)
        {
            //1

            if (pcgrid[Metro.opening[k, 0], Metro.opening[k, 1]] != 1)

            {
                pcgrid[Metro.opening[k, 0], Metro.opening[k, 1]] = 3;
                guardarDir[Metro.opening[k, 0], Metro.opening[k, 1]] = (byte)Metro.opening[k, 2];

            }
        }
    }

    public void initRooms()
    {

        rooms = new ArrayList(); // New room arraylist
        for (int n = 0; n < roomMax; n++) // tenta meter quartos ate atingir limite
        {
            room_blocked = false; // Unblock
            if (!buildMetro) // meter metro e cela
            {
                insertMetro();
                insertCell();
                buildMetro = true;
            }
            else { 
            Room rm = new Room(pcgrid_width, pcgrid_height, room_base, room_radix, corridor_num, R, 0, 0, 0,
                Vector2.zero); // Criar Room
            room_blocked = blockRoom(rm); // ve se e possivel

            if (room_blocked)
            {
                n--; // Remake room
                    tenativas--; // Stops if taking too long
                if (tenativas == 0)
                {
                    room_num--;
                        tenativas = 3500; // Recursion limit
                }
            }
            else
            {
                rooms.Add(rm);

                // Create room
                for (int j = rm.room_y1; j <= rm.room_y2; j++)
                {
                    for (int i = rm.room_x1; i <= rm.room_x2; i++)
                    {
                        pcgrid[i, j] = 1;
                    }
                }
                // Create room walls
                for (int i = rm.wall_x1; i <= rm.wall_x2; i++)
                {
                    if (pcgrid[i, rm.wall_y1] != 1) //wall de baixo
                        {
                            pcgrid[i, rm.wall_y1] = 2;
                            guardarDir[i, rm.wall_y1] = 4;

                        }
                        if (pcgrid[i, rm.wall_y2] != 1) {
                            pcgrid[i, rm.wall_y2] = 2;
                            guardarDir[i, rm.wall_y2] = 6;

                        }
                    }
                for (int j = rm.wall_y1; j <= rm.wall_y2; j++)
                {
                    if (pcgrid[rm.wall_x1, j] != 1) {
                            pcgrid[rm.wall_x1, j] = 2;
                            guardarDir[rm.wall_x1, j] = 7;
                        }
                        if (pcgrid[rm.wall_x2, j] != 1)
                        {
                            pcgrid[rm.wall_x2, j] = 2;
                            guardarDir[rm.wall_x2, j] = 5;
                        }
                    }

                guardarDir[rm.wall_x1, rm.wall_y2] = 9;
                guardarDir[rm.wall_x2, rm.wall_y2] = 10;


                // Place openings
                for (int k = 0; k < rm.opening_num; k++)
                {
                    //1

                    if (pcgrid[rm.opening[k, 0], rm.opening[k, 1]] != 1)
                        {
                            pcgrid[rm.opening[k, 0], rm.opening[k, 1]] = 3; // coordenadaX , coordenadaY, porta 
                            guardarDir[rm.opening[k, 0], rm.opening[k, 1]] = (byte)rm.opening[k, 2];
                        }
                    }
            }
        }
        } // metro else
    }

    private bool blockRoom(Room rm)
    {

        // If outside of grid bounded metodo do pai PCG ve se X,Y <0 e se esta fora dos limites do tamanho do mapa
        if (!bounded(rm.wall_x1, rm.wall_y1) || !bounded(rm.wall_x2, rm.wall_y1) ||
            !bounded(rm.wall_x1, rm.wall_y2) || !bounded(rm.wall_x2, rm.wall_y2))
        {
            return true;
        }
        // If blocked by another room
        if (room_type != 3)
        {
            for (int i = rm.wall_x1 - 1; i < rm.wall_x2 + 1; i++)
            {
                // Check upper and lower bound  espaco de 3 espacos entre salas
                if (bounded(i, rm.wall_y1 - 1) && !blocked(i, rm.wall_y1 - 1, 0)) return true;
                if (bounded(i, rm.wall_y2 + 1) && !blocked(i, rm.wall_y2 + 1, 0)) return true;
                if (bounded(i, rm.wall_y1 - 2) && !blocked(i, rm.wall_y1 - 2, 0)) return true;
                if (bounded(i, rm.wall_y2 + 2) && !blocked(i, rm.wall_y2 + 2, 0)) return true;
                if (bounded(i, rm.wall_y1 - 3) && !blocked(i, rm.wall_y1 - 3, 0)) return true;
                if (bounded(i, rm.wall_y2 + 3) && !blocked(i, rm.wall_y2 + 3, 0)) return true;
            }
            for (int j = rm.wall_y1 - 1; j < rm.wall_y2 + 1; j++)
            {
                // Check left and right deixa espaco de 3 espacos entre salas
                if (bounded(rm.wall_x1 - 1, j) && !blocked(rm.wall_x1 - 1, j, 0)) return true;
                if (bounded(rm.wall_x2 + 1, j) && !blocked(rm.wall_x2 + 1, j, 0)) return true;
                if (bounded(rm.wall_x1 - 2, j) && !blocked(rm.wall_x1 - 2, j, 0)) return true;
                if (bounded(rm.wall_x2 + 2, j) && !blocked(rm.wall_x2 + 2, j, 0)) return true;
                if (bounded(rm.wall_x1 - 3, j) && !blocked(rm.wall_x1 - 3, j, 0)) return true;
                if (bounded(rm.wall_x2 + 3, j) && !blocked(rm.wall_x2 + 3, j, 0)) return true;
            }
        }
        return false;
    }

    private void initCorridors()
    {
       // Debug.Log("test");

        if (room_type != 3) // 0-2
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                // Go through each room and connect its first opening to the first opening of the next room
                Room rm1 = (Room)rooms[i];
                Room rm2;
                if (i == rooms.Count - 1)
                    rm2 = (Room)rooms[0];
                else
                    rm2 = (Room)rooms[i + 1]; // If not last room

                // Connect rooms
                basicAStar(pcgrid, rm1.opening[0, 0], rm1.opening[0, 1], rm2.opening[0, 0], rm2.opening[0, 1], corridor_weight, turning_weight);

                // Random tunneling
                //for (int j = 1; j < rm1.opening_num; j++)
                //{
                //    Debug.Log("Random TUNNEL0!");
                //    tunnelRandom(rm1.opening[j, 0], rm1.opening[j, 1], rm1.opening[j, 2], 3);
                //}
            }
        }
        else
        { // If complex
            Room rm1 = (Room)rooms[0];
            for (int i = 1; i < rooms.Count; i++)
            {
                // Go through each room and connect its first opening to the first opening of the first room
                Room rm2 = (Room)rooms[i];
                // Connect rooms
                basicAStar(pcgrid, rm1.opening[0, 0], rm1.opening[0, 1], rm2.opening[0, 0], rm2.opening[0, 1], corridor_weight, turning_weight);
            }
            // Random tunneling
            for (int i = 0; i < rooms.Count; i++)
            {
                Room rm3 = (Room)rooms[i];
                for (int j = 1; j < rm3.opening_num; j++)
                {
                    Debug.Log("DIR " + rm3.opening[j, 2]);
                    Debug.Log("Random TUNNEL1!");

                    tunnelRandom(rm3.opening[j, 0], rm3.opening[j, 1], rm3.opening[j, 2], 3);
                }
            }
        }
    }
    private void tunnelRandom(int x, int y, int dir, int iteration)
    {
        if (iteration == 0) return; // End of recursion iteration
        Debug.Log("test xx");

        // Choose a random direction and check to see if that cell is occupied, if not, head in that direction
        switch (dir)
        {

            case 0:
                Debug.Log("DIR " + dir);
                //MonoBehaviour.print(dir);
                if (!blockCorridor(x, y - 1, 0)) tunnel(x, y - 1, dir); // North
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 1:
                Debug.Log("DIR " + dir);
                //MonoBehaviour.print(dir);

                if (!blockCorridor(x + 1, y, 1)) tunnel(x + 1, y, dir); // East
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 2:
                Debug.Log("DIR " + dir);
                //MonoBehaviour.print(dir);

                if (!blockCorridor(x, y + 1, 0)) tunnel(x, y + 1, dir); // South
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 3:
                Debug.Log("DIR " + dir);
                //MonoBehaviour.print(dir);

                if (!blockCorridor(x - 1, y, 1)) tunnel(x - 1, y, dir); // West
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
        }
    }

    private void tunnel(int x, int y, int dir)
    {
        if (pcgrid[x, y] == 2 || pcgrid[x, y] == 3) pcgrid[x, y] = 3; // If on top of wall or door
        else
        {
            pcgrid[x, y] = 4; // Set cell to corridor

            //switch (dir)
            //{ // 1east  3west 0 north 2 south
            //    case  0:
            //        if (pcgrid[x + 2, y] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");
            //            pcgrid[x + 2, y] = 4;
            //        }
            //        else if (pcgrid[x - 2, y] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x - 2, y] = 4;

            //        }
            //        break;
            //    case 1: // east
            //        if (pcgrid[x, y + 2] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //             pcgrid[x, y + 2] = 4;
            //        }
            //        else if (pcgrid[x, y - 2] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x, y - 2] = 4;

            //        }

            //        break;
            //    case 2: // south\
            //        if (pcgrid[x + 2, y] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x + 2, y] = 4;
            //        }
            //        else if (pcgrid[x - 2, y] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x - 2, y] = 4;

            //        }
            //        break;
            //    case 3:
            //        if (pcgrid[x, y +2] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x, y + 2] = 4;
            //        }
            //        else if (pcgrid[x, y - 2] == 0 && bounded(x, y))
            //        {
            //            Debug.Log("test");

            //            pcgrid[x, y - 2] = 4;

            //        }

            //        break;
            //}
            tunnelRandom(x, y, shuffleDir(dir, 85), 3); // Randomly choose next cell to go to
        }
    }

    private float NextFloat(System.Random random, float min, float max)
    {

        return min + ((max - min) * (float)(random.NextDouble()));
    }
    
    private int shuffleDir(int dir, int prob)
    {


        // Randomly choose direction based on probability
        if ((NextFloat(R,0,100)) > (100 - prob))
        {
            return dir; // Stay same direction
        }
        else
        { // Change direction
            switch (dir)
            {

                case 0: if ((int)(NextFloat(R, 0, 100)) < 50) return 1; // East
                    if ((int)(NextFloat(R,0,100)) >= 50) return 3; // West
                    break;
                case 1: if ((int)(NextFloat(R,0,100)) < 50) return 0; // North
                    if ((int)(NextFloat(R,0,100)) >= 50) return 2; // South
                    break;
                case 2: if ((int)(NextFloat(R, 0, 100)) < 50) return 1; // East
                    if ((int)(NextFloat(R,0,100)) >= 50) return 3; // West
                    break;
                case 3: if ((int)(NextFloat(R, 0, 100)) < 50) return 0; // North
                    if ((int)(NextFloat(R,0,100)) >= 50) return 2; // South
                    break;


            }
        }
        return dir;
    }

    bool blockCorridor(int x, int y, int orientation)
    {
        Debug.Log("Block COrridor!");
        if (!bounded(x, y)) return true; // If outside of grid

        // Check if current cell is available as corridor based on previous corridor cell location
        switch (orientation)
        {
            // N/S
            case 0: if (blocked(x, y, 1) || // Blocked by room
                        (blocked(x - 1, y, 4) && blocked(x - 1, y + 1, 4)) || // Next to corridor
                        (blocked(x - 1, y, 4) && blocked(x - 1, y - 1, 4)) || // Next to corridor
                        (blocked(x + 1, y, 4) && blocked(x + 1, y + 1, 4)) || // Next to corridor
                        (blocked(x + 1, y, 4) && blocked(x + 1, y - 1, 4)) || // Next to corridor
                        ((blocked(x, y, 2) || blocked(x, y, 3)) && (((blocked(x, y - 1, 2) || blocked(x, y - 1, 3)) && (blocked(x + 1, y, 2) || blocked(x + 1, y, 2))) ||
                                                                   ((blocked(x, y - 1, 2) || blocked(x, y - 1, 3)) && (blocked(x - 1, y, 2) || blocked(x - 1, y, 2))) ||
                                                                   ((blocked(x, y + 1, 2) || blocked(x, y + 1, 3)) && (blocked(x + 1, y, 2) || blocked(x + 1, y, 2))) ||
                                                                   ((blocked(x, y + 1, 2) || blocked(x, y + 1, 3)) && (blocked(x - 1, y, 2) || blocked(x - 1, y, 2))))))
                    return true;
                break;
            // W/E
            case 1: if (blocked(x, y, 1) || // Blocked by room
                        (blocked(x, y - 1, 4) && blocked(x - 1, y + 1, 4)) || // Next to corridor
                        (blocked(x, y - 1, 4) && blocked(x - 1, y - 1, 4)) || // Next to corridor
                        (blocked(x, y + 1, 4) && blocked(x + 1, y + 1, 4)) || // Next to corridor
                        (blocked(x, y + 1, 4) && blocked(x + 1, y - 1, 4)) || // Next to corridor
                        ((blocked(x, y, 2) || blocked(x, y, 3)) && (((blocked(x, y - 1, 2) || blocked(x, y - 1, 3)) && (blocked(x + 1, y, 2) || blocked(x + 1, y, 2))) ||
                                                                   ((blocked(x, y - 1, 2) || blocked(x, y - 1, 3)) && (blocked(x - 1, y, 2) || blocked(x - 1, y, 2))) ||
                                                                   ((blocked(x, y + 1, 2) || blocked(x, y + 1, 3)) && (blocked(x + 1, y, 2) || blocked(x + 1, y, 2))) ||
                                                                   ((blocked(x, y + 1, 2) || blocked(x, y + 1, 3)) && (blocked(x - 1, y, 2) || blocked(x - 1, y, 2))))))
                    return true;
                break;
        }

        return false;
    }

    List<int> listaDir = new List<int>();
    // x1 y1 porta de um quarto x2 y2 porta para o segundo quarto
    private void basicAStar(byte[,] pcgrid, int x1, int y1, int x2, int y2, int corr_wt, int trn_wt)
    {
        int grid_w = pcgrid.GetLength(0);
        int grid_h = pcgrid.GetLength(1);

        //Debug.Log("Corredores");

        byte[, ,] grid = new byte[grid_w, grid_h, 3];


        for (int j = 0; j < grid_h; j++)
        {
            for (int i = 0; i < grid_w; i++)
            {
                // inicializacao das listas 
                grid[i, j, 0] = pcgrid[i, j]; // Cell content
                grid[i, j, 1] = 0; // Open list
                grid[i, j, 2] = 0; // Closed list
            }
        }


        open_list = new BinaryHeap(); // array list
        closed_list = new BinaryHeap();

        // Push node incial na open list
        Node start = new Node(x1, y1, 0, 0, -1);
        Node end = new Node(0, 0, 0, 0, -1);
        open_list.insert(start);

        // Enquanto open list not empty
        while (open_list.getSize() > 0)
        {
            Node current = (Node)open_list.remove(0); // Remove from open list
            grid[current.getX, current.getY, 1] = 0;

            // If at goal
            if (current.getX == x2 && current.getY == y2)
            {
                while (current.getX != x1 || current.getY != y1)
                {
                    if (grid[current.getX, current.getY, 0] == 3){
                        grid[current.getX, current.getY, 0] = 3;
                    }
                    else {
                        grid[current.getX, current.getY, 0] = 4;

                        listaDir.Add(current.getDir);
                        //switch (current.getDir)
                        //{ // 1east  3west 0 north 2 south
                        //    case 0:


                        //        if (!isBlocked(grid, current.getX + 1, current.getY) &&
                        //            grid[current.getX + 1, current.getY, 0] == 0)
                        //        {
                        //            if (!isBlocked(grid, current.getX + 1, current.getY) &&
                        //                grid[current.getX + 1, current.getY, 0] == 0)
                        //            {

                        //            }
                        //        }
                        //        else if (!isBlocked(grid, current.getX - 1, current.getY) &&
                        //                 grid[current.getX + 1, current.getY, 0] == 0)
                        //        {

                        //        }
                        //        if (grid[current.getX + 1, current.getY, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");
                        //            grid[current.getX + 1, current.getY, 0] = 5;
                        //        }
                        //        else if (grid[current.getX - 1, current.getY, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX - 1, current.getY, 0] = 5;

                        //        }
                        //        break;
                        //    case 1: // east
                        //        if (grid[current.getX, current.getY + 1, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX, current.getY + 1, 0] = 5;
                        //        }
                        //        else if (grid[current.getX, current.getY - 2, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX, current.getY - 1, 0] = 5;

                        //        }

                        //        break;
                        //    case 2: // south\
                        //        if (grid[current.getX + 1, current.getY, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX + 1, current.getY, 0] = 5;
                        //        }
                        //        else if (grid[current.getX - 2, current.getY, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX - 1, current.getY, 0] = 5;

                        //        }
                        //        break;
                        //    case 3:
                        //        if (grid[current.getX, current.getY + 1, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX, current.getY + 1, 0] = 5;
                        //        }
                        //        else if (grid[current.getX, current.getY - 2, 0] == 0 && bounded(current.getX, current.getY))
                        //        {
                        //            //Debug.Log("test");

                        //            grid[current.getX, current.getY - 1, 0] = 5;

                        //        }

                        //        break;
                        //}
                        //grid[current.getX, current.getY, 0] = 4;

                    }// Adds corredor
                    current = (Node)current.parent;
                }
                break;
            }

            // Analisa as quartro direcoes futuras para  movimento

            neighbor(grid, current, current.getX, current.getY - 1, x2, y2, 0, corr_wt, trn_wt, current.getDir);

            neighbor(grid, current, current.getX + 1, current.getY, x2, y2, 1, corr_wt, trn_wt, current.getDir);

            neighbor(grid, current, current.getX, current.getY + 1, x2, y2, 2, corr_wt, trn_wt, current.getDir);

            neighbor(grid, current, current.getX - 1, current.getY, x2, y2, 3, corr_wt, trn_wt, current.getDir);

            //Debug.Log("Presumo direcao a ser escolhida5 tem que ter x -1" + current.getDir + " " + current.getX + " " + current.getY);
            closed_list.insert(current); // Add to closed list
            grid[current.getX, current.getY, 2] = 1;
        }

        // Update grid
        for (int j = 0; j < grid_h; j++)
        {
            for (int i = 0; i < grid_w; i++)
            {
                pcgrid[i, j] = grid[i, j, 0];
            }
        }
    }

    private void neighbor(byte[, ,] grid, Node current, int x, int y, int x2, int y2, int dir, int corr_wt, int trn_wt, int currentDirection)
    {
        bool NaoTemEspacoEntreParede = false;


       // Debug.Log("NaoTemEspacoEntreParede " + NaoTemEspacoEntreParede);

        // If not blocked or not in closed list
        if ((!AStarBlocked(grid, x, y, dir, currentDirection) && !NaoTemEspacoEntreParede) && grid[x, y, 2] != 1)
        {
            if (grid[x, y, 1] != 1)
            { // If not in open list
                int g_score = current.getH + 10; // Calculate g score
                if (grid[x, y, 0] == 4 || grid[x, y, 0] == 5) g_score = g_score - corr_wt; // juntar corredor peso
                if (dir == current.getDir) g_score = g_score - trn_wt; // Added weight for keep straight
                int h_score = heuristic(x, y, x2, y2); // Calculate h score
                Node child = new Node(x, y, g_score, h_score, dir);
                child.parent = current; // Assign parent
                open_list.insert(child); // Add to open list
                grid[x, y, 1] = 1;

            }
            else
            { // If already in open list
                int pos = open_list.find(x, y);
                //Node temp = open_list.remove(pos);


                Node temp = (Node)open_list.h[pos];

                // If has better score
                if (current.getG + 10 < temp.getG || (grid[x, y, 0] == 4 && current.getG + (10 - corr_wt) < temp.getG) || (temp.getDir == current.getDir && current.getG + (10 - trn_wt) < temp.getG))
                {
                    temp.getG = current.getG + 10; // Set new g score
                    if (grid[x, y, 0] == 4 || grid[x, y, 0] == 5) temp.getG = temp.getG - corr_wt; // Added weight for joining corridors
                    if (temp.getDir == current.getDir) temp.getG = temp.getG - trn_wt; // Added weight for keep straight
                    temp.getF = temp.getG + temp.getH; // Calculate new f score
                    temp.parent = current; // Assign new parent
                    open_list.moveUp(pos);
                }
                // Insert back to open list
                //open_list.insert(temp);
            }
        }
    }

    private int heuristic(int x, int y, int x2, int y2)
    {
        int h_score = 0;

        h_score = 10 * (Mathf.Abs(x - x2) + Mathf.Abs(y - y2));

        return h_score;
    }

    private bool isBlocked(byte[, ,] grid, int x, int y)
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) return true; // Check if cell is inside grid se estiver esta bloqueada
        if (grid[x, y, 0] == 2) return true; // Check if cell is occupied by wall?
        if (grid[x, y, 0] == 1) return true; // Check if cell is occupied by wall?
        if (grid[x, y, 0] == 3) return true; // Check if cell is occupied by wall?
        if (grid[x, y, 0] == 4) return true; // Check if cell is occupied by wall?

        return false;
    }

    private bool isCorridor(byte[, ,] grid, int x, int y)
    {

        if (grid[x, y, 0] == 4) return true; // Check if cell is occupied by wall?
        if (grid[x, y, 0] == 5) return true; // Check if cell is occupied by wall?

        return false;
    }

    private bool AStarBlocked(byte[, ,] grid, int x, int y, int trueDir, int currentDir)
    {
        bool Xm1 = false, Xp1 = false, Ym1 = false, Yp1 = false;

        if (x + 1 >= pcgrid_width)
            Xp1 = true;
        if (x - 1 < 0)
            Xm1 = true;
        if (y + 1 >= pcgrid_height)
            Yp1 = true;
        if (y - 1 < 0)
            Ym1 = true;



        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
        {
           // Debug.Log("Fora dos limites!");
                    return true; // Check if cell is inside grid se estiver esta bloqueada

        }

        if (grid[x, y, 0] == 3)
        {
            return false;
        }


        if (grid[x, y, 0] == 1)
        {
            return true;
        }
        if (grid[x, y, 0] == 2)
        {
            return true;
        }



        switch (trueDir) // impede os futuros caminhos a uma celula q faca contact com parede
        {
            case 0: //y-1

                    //if (!Xp1 && !Yp1) { 
                    //    if (grid[x + 1, y+1, 0] != 0)
                    //    {
                    //        if (grid[x + 1, y+1, 0] != 4)
                    //            return true;
                    //    }
                    //}
                    //if (!Xm1 && !Xp1)
                    //{
                    //    if (grid[x - 1, y + 1, 0] != 0)
                    //    {
                    //        if (grid[x - 1, y + 1, 0] != 4)
                    //            return true;
                    //    }
                    //}

                    if (!Xm1)
                    {
                        if (grid[x - 1, y, 0] == 3)
                            return false;

                        if (!Ym1)
                        {
                            if (grid[x - 1, y - 1, 0] != 0)
                            {
                                if (grid[x - 1, y - 1, 0] != 4)
                                    return true;
                            }

                        }
                    }


                    if (!Xp1)
                    {
                        if (grid[x + 1, y, 0] == 3)
                            return false;
                        if ( !Ym1)
                        {
                            if (grid[x, y-1, 0] == 3)
                                return false;
                            if (grid[x + 1, y - 1, 0] != 0)
                            {
                                //vou ter q ver se é porta
                                if (grid[x + 1, y - 1, 0] != 4)
                                    return true;
                            }
                        }
                    }

                    ///
                    /// 
                    if(!Xm1)
                    if (grid[x - 1, y, 0] != 0)
                    {
                        if (grid[x - 1, y, 0] != 4)
                            return true;
                    }

                    if (!Xp1)
                    if (grid[x + 1, y, 0] != 0)
                    {
                        if (grid[x + 1, y, 0] != 4)
                            return true;
                    }
                    if (!Ym1)
                    if (grid[x, y - 1, 0] != 0)
                    {
                        if (grid[x, y - 1, 0] != 4)
                            return true;
                    }

                break;
            case 1: // x+1

                    if (!Xp1)
                    {
                        if (grid[x + 1, y, 0] == 3)
                            return false;
                        if (!Yp1) {
                            if (grid[x, y + 1, 0] == 3)
                                return false;
                            if (grid[x + 1, y + 1, 0] != 0)
                            {
                                if (grid[x + 1, y + 1, 0] != 4)
                                    return true;
                            }
                        }
                    }


                        if (!Ym1)
                        {
                            if (grid[x, y - 1, 0] == 3)
                                return false;
                            if (!Xp1)
                            {
                                if (grid[x + 1, y - 1, 0] != 0)
                                {
                                    if (grid[x + 1, y - 1, 0] != 4)
                                        return true;
                                }
                            }
                        }

                        //if (!Ym1)
                        //{
                        //    if (grid[x, y - 1, 0] == 3)
                        //    {
                        //        return false;
                        //    }
                        //    if(!Xm1){
                        //        if (grid[x - 1, y - 1, 0] != 0)
                        //        {
                        //            if (grid[x - 1, y - 1, 0] != 4)
                        //                return true;
                        //        }
                        //    }
                        ////}

                        //    if (!Xp1)
                        //    {

                        //        if (!Xm1) { 
                        //            if (grid[x - 1, y + 1, 0] != 0)
                        //            {
                        //                if (grid[x - 1, y + 1, 0] != 4)
                        //                    return true;
                        //            }
                        //        }
                        //    }
                    //Distancia de uma celula na esquina
                if(!Xp1)
                    if (grid[x + 1, y, 0] != 0)
                    {
                        if (grid[x + 1, y, 0] != 4)
                            return true;
                    }
                if (!Ym1)
                    if (grid[x, y - 1, 0] != 0)
                    {
                        if (grid[x, y - 1, 0] != 4)
                            return true;
                    }
                if (!Yp1)
                    if (grid[x, y + 1, 0] != 0)
                    {
                        if (grid[x, y + 1, 0] != 4)
                            return true;
                    }

                break;

            case 2://Y + 1
                    if (!Xp1) {
                        if (grid[x + 1, y, 0] == 3)
                        {
                            return false;
                        }
                        if (!Yp1) {
                            if (grid[x, y + 1, 0] == 3)
                            {
                                return false;
                            }
                            if (grid[x + 1, y + 1, 0] != 0)
                            {
                                if (grid[x + 1, y + 1, 0] != 4)
                                    return true;
                            }
                        }
                    }
                    //if (grid[x + 1, y - 1, 0] != 0)
                    //{
                    //    if (grid[x + 1, y - 1, 0] != 4)
                    //        return true;
                    //}
                    if (!Xm1)
                    {
                        if (grid[x - 1, y, 0] == 3)
                            return false;
                        if(!Yp1)
                        if (grid[x - 1, y + 1, 0] != 0)
                        {
                            if (grid[x - 1, y + 1, 0] != 4)
                                return true;
                        }
                    }

                    //if (grid[x - 1, y - 1, 0] != 0)
                    //{
                    //    if (grid[x - 1, y - 1, 0] != 4)
                    //        return true;
                    //}

                    //
                if(!Xp1)
                    if (grid[x + 1, y, 0] != 0)
                    {
                        if (grid[x + 1, y, 0] != 4)
                            return true;
                    }
                if (!Xm1)
                    if (grid[x - 1, y, 0] != 0)
                    {
                        if (grid[x - 1, y, 0] != 4)
                            return true;
                    }
                if(!Yp1)
                    if (grid[x, y + 1, 0] != 0)
                    {
                        if (grid[x, y + 1, 0] != 4)
                            return true;
                    }
                break;
            case 3://X - 1

                    //if (grid[x + 1, y + 1, 0] != 0)
                    //{
                    //    if (grid[x + 1, y + 1, 0] != 4)
                    //        return true;
                    //}

                    //if (grid[x + 1, y - 1, 0] != 0)
                    //{
                    //    if (grid[x + 1, y - 1, 0] != 4)
                    //        return true;
                    //}

                    if (!Xm1)
                    {

                        if (grid[x - 1, y, 0] == 3)
                            return false;
                        //ve primeiro se pode ir 2 casas para a frente
                        if (grid[x - 1, y, 0] != 0)
                        {
                            if (grid[x - 1, y, 0] != 4)
                                return true;
                        }

                        if (!Yp1)
                        {
                            // se pode ir para Y positivo 
                            if (grid[x, y + 1, 0] == 3)
                                return false;

                            if (grid[x - 1, y + 1, 0] != 0)
                            {
                                if (grid[x - 1, y + 1, 0] != 4)
                                    return true;
                            }
                        }
                    }

                    if (!Ym1)
                    {
                        if (grid[x, y - 1, 0] == 3)
                        {
                            return false;
                        }
                        if (!Xm1) { 
                            if (grid[x - 1, y - 1, 0] != 0)
                            {
                                if (grid[x - 1, y - 1, 0] != 4)
                                    return true;
                            }
                        }
                    }


                    //
                
                    if(!Ym1)
                    if (grid[x, y - 1, 0] != 0)
                    {
                        if (grid[x, y - 1, 0] != 4)
                            return true;
                    }
                    if (!Yp1)
                    if (grid[x, y + 1, 0] != 0)
                    {
                        if (grid[x, y + 1, 0] != 4)
                            return true;
                    }
                break;
            default:
                break;
        }


        return false;
        }
}
//switch (trueDir) //direcao futura ve se permite andar uma celula
//    {
//        case 0:

//            if (grid[x, y - 1, 0] != 0) // se nao for 3 ou 4 e diferent de 0 entao nao pode
//                return true;
//            break;
//        case 1:

//            if (grid[x + 1, y, 0] != 0) // se nao for 3 ou 4 e diferent de 0 entao nao pode
//                return true;
//            break;
//        case 2:

//            if (grid[x, y + 1, 0] != 0) // se nao for 3 ou 4 e diferent de 0 entao nao pode
//                return true;
//            break;
//        case 3:

//            if (grid[x - 1, y, 0] != 0) // se nao for 3 ou 4 e diferent de 0 entao nao pode
//                return true;
//            break;
//    }