using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        // Saves all points that have been visited
        private List<Point> VisitedPoints = new List<Point>();
        private bool reachedEnd = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            // The robot starts at 0,0
            int x = 0;
            int y = 0;

            robot.ReachedExit += (_, __) => reachedEnd = true;

            SolveMaze(x, y);

            // The robot couldn't find an escape
            if (!reachedEnd)
            {
                robot.HaltAndCatchFire();
            }
        }
        
        public void SolveMaze(int x, int y)
        {
            // The point that is currently being checked
            Point currentPoint = new Point(x, y);
            if (!VisitedPoints.Contains(currentPoint) && !reachedEnd)
            {
                VisitedPoints.Add(currentPoint);

                if (!reachedEnd && robot.TryMove(Direction.Up))
                {
                    SolveMaze(x, y - 1);
                    if (!reachedEnd)
                    {
                        robot.Move(Direction.Down);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Down))
                {
                    SolveMaze(x, y + 1);
                    if (!reachedEnd)
                    {
                        robot.Move(Direction.Up);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Right))
                {
                    SolveMaze(x + 1, y);
                    if (!reachedEnd)
                    {
                        robot.Move(Direction.Left);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Left))
                {
                    SolveMaze(x - 1, y);
                    if (!reachedEnd)
                    {
                        robot.Move(Direction.Right);
                    }
                }
            }
        }
    }
}
