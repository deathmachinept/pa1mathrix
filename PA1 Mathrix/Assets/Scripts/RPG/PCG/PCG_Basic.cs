using UnityEngine;
using System.Collections;

public class PCG_Basic : PCG{

    public int layout_type; // Layout type
    public int room_type; // Room type
    public int room_max; // Max room size
    public int room_min; // Min room size
    public int room_num; // Number of rooms
    private int room_base;
    private int room_radix;
    private bool room_blocked = false; // If room is blocked
    int redo = 1000; // Recursion limit
    ArrayList rooms; // Room arraylist
    int corridor_num;
    int corridor_weight;
    int turning_weight;

    // *A Star
    BinaryHeap open_list;
    BinaryHeap closed_list;

    public void updateParam(int g_width, int g_height, int r_type, int r_min, int r_max, int c_num, int c_weight, int t_weight)
    {
        base.updateParam(g_width, g_height);
        updateParam(g_width, g_height);

        room_type = r_type; // Room type

        room_min = r_min; // Default 9
        room_max = r_max; // Default 16
        room_base = (int)((room_min + 1) * 0.5);
        room_radix = (int)((room_max - room_min) * 0.5 + 1);



        switch (room_type)
        {
            case 0: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max) + 1;
                break; // Scattered
            case 1: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max * 2) + 1;
                break; // Sparse
            case 2: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_min * 0.5) + 1;
                break; // Dense
            default: room_num = (pcgrid_width * pcgrid_height) / (int)(Random.Range(room_min, room_max) * room_max) + 1;
                break; // Scattered
        }

        corridor_num = c_num;
        corridor_weight = c_weight;
        turning_weight = t_weight;
    }

    public void generatePCGBasic(byte[,] g)
    {
        base.generatePCG(g); // Init grid 

        initRooms(); // Initialize rooms
        initCorridors(); // Initialize corridors
    }


    public void initRooms()
    {
        rooms = new ArrayList(); // New room arraylist
        for (int n = 0; n < room_num; n++)
        {
            room_blocked = false; // Unblock
            Room rm = new Room(pcgrid_width, pcgrid_height, room_base, room_radix, corridor_num); // Create new room
            room_blocked = blockRoom(rm); // Check if room is blocked

            if (room_blocked)
            {
                n--; // Remake room
                redo--; // Stops if taking too long
                if (redo == 0)
                {
                    room_num--;
                    redo = 1000; // Recursion limit
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
                    if (pcgrid[i, rm.wall_y1] != 1) pcgrid[i, rm.wall_y1] = 2;
                    if (pcgrid[i, rm.wall_y2] != 1) pcgrid[i, rm.wall_y2] = 2;
                }
                for (int j = rm.wall_y1; j <= rm.wall_y2; j++)
                {
                    if (pcgrid[rm.wall_x1, j] != 1) pcgrid[rm.wall_x1, j] = 2;
                    if (pcgrid[rm.wall_x2, j] != 1) pcgrid[rm.wall_x2, j] = 2;
                }
                // Place openings
                for (int k = 0; k < rm.opening_num; k++)
                {
                    if (pcgrid[rm.opening[k, 0], rm.opening[k, 1]] != 1) pcgrid[rm.opening[k, 0], rm.opening[k, 1]] = 3;
                }
            }
        }
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
                // Check upper and lower bound
                if (bounded(i, rm.wall_y1 - 1) && !blocked(i, rm.wall_y1 - 1, 0)) return true;
                if (bounded(i, rm.wall_y2 + 1) && !blocked(i, rm.wall_y2 + 1, 0)) return true;
            }
            for (int j = rm.wall_y1 - 1; j < rm.wall_y2 + 1; j++)
            {
                // Check left and right bound
                if (bounded(rm.wall_x1 - 1, j) && !blocked(rm.wall_x1 - 1, j, 0)) return true;
                if (bounded(rm.wall_x2 + 1, j) && !blocked(rm.wall_x2 + 1, j, 0)) return true;
            }
        }
        return false;
    }

    private void initCorridors()
    {
        if (room_type != 3)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                // Go through each room and connect its first opening to the first opening of the next room
                Room rm1 = (Room)rooms[i];
                Room rm2;
                if (i == rooms.Count - 1) rm2 = (Room)rooms[0];
                else rm2 = (Room)rooms[i + 1]; // If not last room

                // Connect rooms
                basicAStar(pcgrid, rm1.opening[0, 0], rm1.opening[0, 1], rm2.opening[0, 0], rm2.opening[0, 1], corridor_weight, turning_weight);

                // Random tunneling
                for (int j = 1; j < rm1.opening_num; j++)
                {
                    tunnelRandom(rm1.opening[j, 0], rm1.opening[j, 1], rm1.opening[j, 2], 3);
                }
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
                    tunnelRandom(rm3.opening[j, 0], rm3.opening[j, 1], rm3.opening[j, 2], 3);
                }
            }
        }
    }
    private void tunnelRandom(int x, int y, int dir, int iteration)
    {
        if (iteration == 0) return; // End of recursion iteration

        // Choose a random direction and check to see if that cell is occupied, if not, head in that direction
        switch (dir)
        {
            case 0: if (!blockCorridor(x, y - 1, 0)) tunnel(x, y - 1, dir); // North
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 1: if (!blockCorridor(x + 1, y, 1)) tunnel(x + 1, y, dir); // East
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 2: if (!blockCorridor(x, y + 1, 0)) tunnel(x, y + 1, dir); // South
                else tunnelRandom(x, y, shuffleDir(dir, 0), iteration - 1); // Try again
                break;
            case 3: if (!blockCorridor(x - 1, y, 1)) tunnel(x - 1, y, dir); // West
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
            tunnelRandom(x, y, shuffleDir(dir, 85), 3); // Randomly choose next cell to go to
        }
    }

    private int shuffleDir(int dir, int prob)
    {

        // Randomly choose direction based on probability
        if (((int)Random.Range(0f, 100)) > (100 - prob))
        {
            return dir; // Stay same direction
        }
        else
        { // Change direction
            switch (dir)
            {
                case 0: if ((int)(Random.Range(0f, 100)) < 50) return 1; // East
                    if ((int)(Random.Range(0f, 100)) >= 50) return 3; // West
                    break;
                case 1: if ((int)(Random.Range(0f, 100)) < 50) return 0; // North
                    if ((int)(Random.Range(0f, 100)) >= 50) return 2; // South
                    break;
                case 2: if ((int)(Random.Range(0f, 100)) < 50) return 1; // East
                    if ((int)(Random.Range(0f, 100)) >= 50) return 3; // West
                    break;
                case 3: if ((int)(Random.Range(0f, 100)) < 50) return 0; // North
                    if ((int)(Random.Range(0f, 100)) >= 50) return 2; // South
                    break;
            }
        }
        return dir;
    }

    bool blockCorridor(int x, int y, int orientation)
    {
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



    private void basicAStar(byte[,] pcgrid, int x1, int y1, int x2, int y2, int corr_wt, int trn_wt)
    {
        int grid_w = pcgrid.GetLength(0);
        int grid_h = pcgrid.GetLength(1);


        byte[, ,] grid = new byte[grid_w, grid_h, 3];

        //         byte[][][] grid = new byte[grid_w][grid_h];

        //byte[][][] grid = new byte[grid_w][grid_h][3];
        // grid[grid_w][][];

        for (int j = 0; j < grid_h; j++)
        {
            for (int i = 0; i < grid_w; i++)
            {
                grid[i, j, 0] = pcgrid[i, j]; // Cell content
                grid[i, j, 1] = 0; // Open list
                grid[i, j, 2] = 0; // Closed list
            }
        }

        open_list = new BinaryHeap();
        closed_list = new BinaryHeap();

        // Push starting node into open list
        Node start = new Node(x1, y1, 0, 0, -1);
        Node end = new Node(0, 0, 0, 0, -1);
        open_list.insert(start);

        // While open list is not empty
        while (open_list.getSize() > 0)
        {
            Node current = (Node)open_list.remove(0); // Remove from open list
            grid[current.getX, current.getY, 1] = 0;

            // If at goal
            if (current.getX == x2 && current.getY == y2)
            {
                while (current.getX != x1 || current.getY != y1)
                {
                    if (grid[current.getX, current.getY, 0] == 3) grid[current.getX, current.getY, 0] = 3;
                    else grid[current.getX, current.getY, 0] = 4;
                    current = (Node)current.parent;
                }
                break;
            }

            // Process neighbors
            neighbor(grid, current, current.getX, current.getY - 1, x2, y2, 0, corr_wt, trn_wt);
            neighbor(grid, current, current.getX + 1, current.getY, x2, y2, 1, corr_wt, trn_wt);
            neighbor(grid, current, current.getX, current.getY + 1, x2, y2, 2, corr_wt, trn_wt);
            neighbor(grid, current, current.getX - 1, current.getY, x2, y2, 3, corr_wt, trn_wt);

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

    private void neighbor(byte[, ,] grid, Node current, int x, int y, int x2, int y2, int dir, int corr_wt, int trn_wt)
    {
        // If not blocked or not in closed list
        if (!AStarBlocked(grid, x, y) && grid[x, y, 2] != 1)
        {
            if (grid[x, y, 1] != 1)
            { // If not in open list
                int g_score = current.getH + 10; // Calculate g score
                if (grid[x, y, 0] == 4) g_score = g_score - corr_wt; // Added weight for joining corridors
                if (dir == current.getDir) g_score = g_score - trn_wt; // Added weight for keep straight
                int h_score = heuristic(x, y, x2, y2, 0); // Calculate h score
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
                    if (grid[x, y, 0] == 4) temp.getG = temp.getG - corr_wt; // Added weight for joining corridors
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

    private int heuristic(int x, int y, int x2, int y2, int method)
    {
        int h_score = 0;
        switch (method)
        {
            case 0: h_score = 10 * (Mathf.Abs(x - x2) + Mathf.Abs(y - y2));
                break; // Manhattan
            case 1: int xDistance = Mathf.Abs(x - x2);
                int yDistance = Mathf.Abs(y - y2);
                if (xDistance > yDistance) h_score = 14 * yDistance + 10 * (xDistance - yDistance);
                else h_score = 14 * xDistance + 10 * (yDistance - xDistance);
                break; // Diagonal
        }
        return h_score;
    }

    private bool AStarBlocked(byte[, ,] grid, int x, int y)
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) return true; // Check if cell is inside grid
        if (grid[x, y, 0] == 1) return true; // Check if cell is occupied by room
        if (grid[x, y, 0] == 2) return true; // Check if cell is occupied by corridor
        return false;
    }
}
