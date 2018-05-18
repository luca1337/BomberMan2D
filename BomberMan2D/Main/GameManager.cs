using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using System.Windows.Forms;
using BehaviourEngine.Pathfinding;

namespace BomberMan2D
{
    public class GameManager : GameObject
    {
        private IState currentState;

        private static List<IWaypoint> targetPoints = new List<IWaypoint>();

        internal static List<IWaypoint> GetAllPoints()
        {
            return targetPoints;
        }

        internal static void AddTargetPoint(IWaypoint current)
        {
            targetPoints.Add(current);
        }

        internal static int GetPointsCount()
        {
            return targetPoints.Count();
        }

        public GameManager() : base("GameManager")
        {
            SetupState setupState = new SetupState(this);
            MenuState menuState = new MenuState(this);
            SinglePlayer singleMode = new SinglePlayer(this);
            PauseState pauseState = new PauseState(this);
            LoseState loseState = new LoseState(this);
            WinState winState = new WinState(this);
            MultiPlayer multiMode = new MultiPlayer(this);
            LobbySetup lobby = new LobbySetup(this);
            //Link State
            setupState.MenuState = menuState;

            menuState.SinglePlayerMode = singleMode;
            menuState.Lobby = lobby;

            singleMode.NextWin = winState;
            singleMode.NextLose = loseState;
            singleMode.NextPause = pauseState;

            lobby.Menu = menuState;
            lobby.MultiMode = multiMode;

            multiMode.NextLose = loseState;
            multiMode.NextPause = pauseState;
            multiMode.NextWin = winState;

            loseState.Retry = menuState;

            setupState.OnStateEnter();
            currentState = setupState;

            AddComponent(new FSMUpdater(currentState));
        }

        private static void SetupCollisionsRulesAndPhysics()
        {
            //do we want physics?
            Physics.Instance.Gravity *= 2f;

            LayerManager.AddLayer((uint)CollisionLayer.BomberMan, (uint)CollisionLayer.SolidWall | (uint)CollisionLayer.Powerup  | (uint)CollisionLayer.Explosion | (uint)CollisionLayer.Bombs);
            LayerManager.AddLayer((uint)CollisionLayer.Explosion, (uint)CollisionLayer.SolidWall);
            LayerManager.AddLayer((uint)CollisionLayer.Enemy,(uint)CollisionLayer.Enemy | (uint)CollisionLayer.Explosion | (uint)CollisionLayer.Bombs);
         //   LayerManager.AddLayer ( (uint)CollisionLayer.Bombs , (uint)CollisionLayer.BomberMan);
        }

        private static void SetupTextures()
        {
            FlyWeight.Add("Font01", "Assets/Font.dat");
            FlyWeight.Add("Box2D", "Assets/Box2D.dat");
            FlyWeight.Add("Wall", "Assets/Wall_01.dat");
            FlyWeight.Add("Obstacle", "Assets/Obstacle_01.dat");
            FlyWeight.Add("BomberMan", "Assets/Bombertab1.dat");
            FlyWeight.Add("Bomb", "Assets/Bomb.dat");
            FlyWeight.Add("Explosion", "Assets/Explosion.dat");
            FlyWeight.Add("AI", "Assets/ballon.dat");
            FlyWeight.Add("Bomb_PW", "Assets/BombsPw.dat");
            FlyWeight.Add("Bombpass_PW", "Assets/BombpassPw.dat");
            FlyWeight.Add("Flamepass_PW", "Assets/FlamepassPw.dat");
            FlyWeight.Add("Mystery_PW", "Assets/MysteryPw.dat");
            FlyWeight.Add("Detonator_PW", "Assets/DetonatorPw.dat");
            FlyWeight.Add("Wallpass_PW", "Assets/WallpassPw.dat");
            FlyWeight.Add("Speed_PW", "Assets/SpeedPw.dat");
            FlyWeight.Add("Flame_PW", "Assets/FlamesPw.dat");
            FlyWeight.Add("MainScreen", "Assets/mainscreen.dat");
            FlyWeight.Add("Lose", "Assets/Lose.dat");
            FlyWeight.Add("Balloom", "Assets/Balloom.dat");
            FlyWeight.Add("Oneal", "Assets/Oneal.dat");
            FlyWeight.Add("Doll", "Assets/Doll.dat");
            FlyWeight.Add("Kondoria", "Assets/Kondoria.dat");
            FlyWeight.Add("Minvo", "Assets/Minvo.dat");
            FlyWeight.Add("Pass", "Assets/Pass.dat");
            FlyWeight.Add("Ovapi", "Assets/Ovapi.dat");
        }

        private static void SetupObjectPools()
        {
            Pool<Bomb>.Register(() => new Bomb(), 100);
            Pool<Explosion>.Register(() => new Explosion(), 100);
            //Pool<AI>.Register(() => new AI(), 100);

            GlobalFactory<Bomb>.RegisterPool(typeof(Bomb), () => new Bomb());
            //powerups
            //Pool<PowerUp>.Register(() => new PowerUp(), 50);

        }

        private static void SetupSounds()
        {
            AudioManager.AddSource(AudioType.SOUND_EXPLOSION);
            AudioManager.AddClip("Sounds/Explosion.ogg", AudioType.SOUND_EXPLOSION);

            AudioManager.AddSource(AudioType.SOUND_DROP);
            AudioManager.AddClip("Sounds/Drop.ogg", AudioType.SOUND_DROP);

            AudioManager.AddSource(AudioType.SOUND_WALK_FAST);
            AudioManager.AddClip("Sounds/StepFast.ogg", AudioType.SOUND_WALK_FAST);

            AudioManager.AddSource(AudioType.SOUND_WALK_SLOW);
            AudioManager.AddClip("Sounds/StepSlow.ogg", AudioType.SOUND_WALK_SLOW);

            AudioManager.AddSource(AudioType.SOUND_PICKUP);
            AudioManager.AddClip("Sounds/Powerup.ogg", AudioType.SOUND_PICKUP);

            AudioManager.AddSource(AudioType.SOUND_DIE);
            AudioManager.AddClip("Sounds/Dead.ogg", AudioType.SOUND_DIE);

            AudioManager.AddSource(AudioType.SOUND_BACKGROUND);
            AudioManager.AddClip("Sounds/03_Stage_Theme.ogg", AudioType.SOUND_BACKGROUND);
        }



        private class SetupState : IState
        {
            public MenuState MenuState { get; set; }

            private GameManager owner { get; set; }

            public SetupState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                //cazzo
                GameManager.SetupCollisionsRulesAndPhysics();
                GameManager.SetupTextures();
                GameManager.SetupObjectPools();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                MenuState.OnStateEnter();
                return MenuState;
                //  Node.ShowPath();
            }
        }

        private class MenuState : IState
        {
            public SinglePlayer SinglePlayerMode { get; set; }
            public LobbySetup Lobby { get; set; }

            private GameManager owner { get; set; }
            private Menu mainMenu;
            private MenuBackground menuBg;

            public MenuState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                if (mainMenu == null)
                {
                    mainMenu = new Menu();
                    GameManager.Spawn(mainMenu);
                }
                else
                {
                    mainMenu.Active = true;
                    foreach (Component item in mainMenu.Components)
                    {
                        item.Enabled = true;
                    }
                }

                if (menuBg == null)
                {
                    menuBg = new MenuBackground("MainScreen");
                    GameObject.Spawn(menuBg);
                }
                else
                {
                    menuBg.Active = true;
                    foreach (Component item in menuBg.Components)
                    {
                        item.Enabled = true;
                    }
                }


            }

            public void OnStateExit()
            {
                menuBg.Active = false;
                foreach (Component item in menuBg.Components)
                {
                    item.Enabled = false;
                }

                mainMenu.Active = false;
                foreach (Component item in mainMenu.Components)
                {
                    item.Enabled = false;
                }
            }

            public IState OnStateUpdate()
            {
                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Space))
                {
                    this.OnStateExit();
                    if (mainMenu.GetComponent<UpdateMenu>().SinglePlayerMode)
                    {
                        SinglePlayerMode.OnStateEnter();
                        return SinglePlayerMode;
                    }
                    else if (!mainMenu.GetComponent<UpdateMenu>().SinglePlayerMode)
                    {
                        Lobby.OnStateEnter();
                        return Lobby;
                    }

                }

                return this;
            }
        }


        private class SinglePlayer : IState
        {
            public WinState NextWin { get; set; }
            public LoseState NextLose { get; set; }
            public PauseState NextPause { get; set; }

            private GameManager owner { get; set; }
            private Bomberman bomberMan;
            private OnScreenDisplay gui;
            private EnemySpawner enemySpawner;
            private TargetSpawner targetSpawner;
            private Map map;

            private Timer timer;

            public SinglePlayer(GameObject owner)
            {
                this.owner = owner as GameManager;
                timer = new Timer(0.6f);
            }

            public void OnStateEnter()
            {
                LoadLevels();
                LoadGameObjects();
            }

            public void OnStateExit()
            {
                gui.Active = false;
                foreach (Component item in gui.Components)
                {
                    item.Enabled = false;
                }

                targetSpawner.Active = false;
                foreach (Component item in targetSpawner.Components)
                {
                    item.Enabled = false;
                }

                enemySpawner.Active = false;
                foreach (Component item in enemySpawner.Components)
                {
                    item.Enabled = false;
                }

                LevelManager.CurrentMap.Active = false;
                foreach (Component item in LevelManager.CurrentMap.Components)
                {
                    item.Enabled = false;
                }
            }

            public IState OnStateUpdate()
            {
                if (bomberMan.Active == false)
                    timer.Update(false);

                //Node.ShowPath();

                Console.WriteLine(timer.currentTime);
                if (timer.IsOver())
                {
                    timer.Stop(true);
                    OnStateExit();
                    NextLose.OnStateEnter();
                    return NextLose;
                }

                return this;
            }

            private void LoadLevels()
            {
                if (map == null)
                {
                    LevelManager.Add("Levels/Level00.csv");
                }
                else
                {
                    map.Active = true;
                    foreach (Component item in map.Components)
                    {
                        item.Enabled = false;
                    }
                }
            }

            private void LoadGameObjects()
            {
                //OSD
                if (gui == null)
                {
                    gui = new OnScreenDisplay();
                    GameObject.Spawn(gui);
                }
                else
                {
                    gui.Active = true;
                    foreach (Component item in gui.Components)
                    {
                        item.Enabled = false;
                    }
                }

                //Player
                if (bomberMan == null)
                {
                    bomberMan = new Bomberman();
                    GameObject.Spawn(bomberMan, Map.GetPlayerSpawnPoint());
                }
                else
                {
                    bomberMan.Active = true;
                    foreach (Component item in bomberMan.Components)
                    {
                        item.Enabled = true;
                    }
                }

                //TargetPoints
                if (targetSpawner == null)
                {
                    targetSpawner = new TargetSpawner(5, 3.5f);
                    GameObject.Spawn(targetSpawner);
                }
                else
                {
                    targetSpawner.Active = true;
                    foreach (Component item in targetSpawner.Components)
                    {
                        item.Enabled = false;
                    }
                }

                //AI
                if (enemySpawner == null)
                {
                    enemySpawner = new EnemySpawner(bomberMan);
                    GameObject.Spawn(enemySpawner);
                }
                else
                {
                    enemySpawner.Active = true;
                    foreach (Component item in enemySpawner.Components)
                    {
                        item.Enabled = false;
                    }
                }
            }
        }

        private class LobbySetup : IState
        {
            public MultiPlayer MultiMode { get; set; }
            public MenuState Menu { get; set; }
            public GameManager owner;
            private Dictionary<uint, CSteamID> lobbies = new Dictionary<uint, CSteamID>();

            private ulong current_lobbyID;
            private List<CSteamID> lobbyIDS = new List<CSteamID>();
            private Callback<LobbyCreated_t> lobbyCreated;
            private Callback<LobbyMatchList_t> lobbyList;
            private Callback<LobbyEnter_t> lobbyEnter;
            private Callback<LobbyChatMsg_t> lobbyChatMsg;
            private Callback<LobbyDataUpdate_t> lobbyInfo;
            private Callback<LobbyChatUpdate_t> lobbyChatInfo;
            private Callback<P2PSessionRequest_t> connectionInfo;

            public LobbySetup(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                if (!SteamAPI.Init())
                {
                    DialogResult eResult = MessageBox.Show("Coult not initialize Steam API...\n" +
                        "Make sure Steam is not closed\n" +
                        "What do you want to do?\n" +
                        "Press OK to get back to Main Menu\n" +
                        "Press Cancel to Abort the Game.",
                        "Steam API Error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    if (eResult == DialogResult.OK)
                    {
                        //go back to main menu!
                    }
                    else
                    {

                    }

                }

                SteamMatchmaking.AddRequestLobbyListStringFilter("name", "Glukosesirup's game", ELobbyComparison.k_ELobbyComparisonEqual);

                //Creation and management of lobby, the lobbies created must be set with SteamMatchmackig.SetData();
                lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
                lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
                lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
                lobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
                connectionInfo = Callback<P2PSessionRequest_t>.Create(OnNewP2PConnection);


                //To accept a P2P connection
                Callback<P2PSessionRequest_t>.Create( cb =>
                {
                    SteamNetworking.AcceptP2PSessionWithUser(cb.m_steamIDRemote);
                }
                );
                //SteamMatchmaking.RequestLobbyList();

                //start multiplayer game, look for lobbies, if no lobbies are found then create a single lobby and join it


            }

            private void OnGetLobbyInfo(LobbyDataUpdate_t result)
            {
                for (int i = 0; i < lobbyIDS.Count; i++)
                {
                    if (lobbyIDS[i].m_SteamID == result.m_ulSteamIDLobby)
                    {
                        Console.WriteLine("Lobby " + i + " :: " + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "name"));
                        return;
                    }
                }
            }

            private void OnLobbyEntered(LobbyEnter_t result)
            {
                current_lobbyID = result.m_ulSteamIDLobby;

                if (result.m_EChatRoomEnterResponse == 1)
                    Console.WriteLine("Lobby joined!");
                else
                    Console.WriteLine("Failed to join lobby.");
            }

            private void OnGetLobbiesList(LobbyMatchList_t result)
            {
                Console.WriteLine("Found " + result.m_nLobbiesMatching + " lobbies!");
                for (int i = 0; i < result.m_nLobbiesMatching; i++)
                {
                    CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                    lobbyIDS.Add(lobbyID);
                    SteamMatchmaking.RequestLobbyData(lobbyID);
                }
            }

            private void OnLobbyCreated(LobbyCreated_t result)
            {
                if (result.m_eResult == EResult.k_EResultOK)
                    Console.WriteLine("Lobby created -- SUCCESS!");
                else
                    Console.WriteLine("Lobby created -- failure ...");

                string personalName = SteamFriends.GetPersonaName();
                if(SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "name", personalName + "'s game"))
                {
                    Console.WriteLine(SteamMatchmaking.GetLobbyData((CSteamID)result.m_ulSteamIDLobby, "name"));
                }
            }

            private void OnLobbyChatMsg(LobbyChatMsg_t result)
            {
                
            }

            void OnNewP2PConnection(P2PSessionRequest_t result)
            {
                SteamNetworking.AcceptP2PSessionWithUser(result.m_steamIDRemote);
            }

            public void OnStateExit()
            {

            }
            private void SearchForLobby()
            {
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 10);

                Callback<LobbyCreated_t>.Create(cb =>
                {
                    Console.WriteLine("ciaone");
                });

                SteamMatchmaking.RequestLobbyList();

                Callback<LobbyMatchList_t>.Create(x =>
                {
                    Console.WriteLine(x.m_nLobbiesMatching);
                });
            }

            public IState OnStateUpdate()
            {
                GetDataFromBroadcasters();

                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.B))
                {
                    SteamMatchmaking.JoinLobby(lobbyIDS[0]);
                   // SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeInvisible, 2);
                }

                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.C))
                {
                    net_broadcast(1, "ciaone");
                }

                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.D))
                {
                    SteamMatchmaking.RequestLobbyList();
                }

                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.L))
                {
                    string message = "massimoisonni ha joinato la tua vita!";
                    byte[] packet = Encoding.ASCII.GetBytes(message);
                    int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);
                    for (int i = 0; i < numPlayers; i++)
                    {
                        Console.Write(SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyMemberByIndex(lobbyIDS[0], i)));
                        SteamNetworking.SendP2PPacket(SteamMatchmaking.GetLobbyMemberByIndex(lobbyIDS[0], i), packet, (uint)packet.Length, EP2PSend.k_EP2PSendReliable);
                    }


                    //SteamMatchmaking.SendLobbyChatMsg(lobbyIDS[0],packet, packet.Length);
                }

                while (SteamNetworking.IsP2PPacketAvailable(out uint messageSize))
                {
                    byte[] packet = new byte[messageSize];
                    CSteamID bho = CSteamID.Nil;

                    if (SteamNetworking.ReadP2PPacket(packet, messageSize, out uint byteReader, out bho))
                    {

                    }

                    string message = Encoding.ASCII.GetString(packet);
                    Console.WriteLine(message);
                }

                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.H))
                {
                    int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);

                    Console.WriteLine("Number of players currently in lobby : " + numPlayers);
                    for (int i = 0; i < numPlayers; i++)
                    {
                        Console.WriteLine("Player(" + i + ") is " + SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)));
                    }
                }

                SteamAPI.RunCallbacks();

                return this;
            }

            private static void GetDataFromBroadcasters()
            {
                while (SteamNetworking.IsP2PPacketAvailable(out uint msgSize))
                {
                    byte[] msgReceived = new byte[msgSize];
                    CSteamID outID = CSteamID.Nil;

                    if (SteamNetworking.ReadP2PPacket(msgReceived, msgSize, out uint readBytes, out outID))
                    {
                        string realMessage = Encoding.ASCII.GetString(msgReceived);

                        string receiverName = SteamFriends.GetFriendPersonaName(outID);

                        Console.WriteLine(receiverName + " Wrote You: " + realMessage);
                    }
                }
            }

            public void net_broadcast(int TYPE, string message)
            {
            }
        }


        private class MultiPlayer : IState
        {
            public WinState NextWin { get; set; }
            public LoseState NextLose { get; set; }
            public PauseState NextPause { get; set; }

            private GameManager owner { get; set; }
            private List<Bomberman> bomberMans = new List<Bomberman>();

            private OnScreenDisplay gui ;
            private EnemySpawner enemySpawner;
            private TargetSpawner targetSpawner;
            private Map map;

            private Timer timer;

            public MultiPlayer(GameObject owner)
            {
                this.owner = owner as GameManager;
                timer = new Timer(0.6f);
            }

            public void OnStateEnter()
            {
                LoadLevels();
                LoadGameObjects();
            }

            public void OnStateExit()
            {
                gui.Active = false;
                foreach (Component item in gui.Components)
                {
                    item.Enabled = false;
                }

                targetSpawner.Active = false;
                foreach (Component item in targetSpawner.Components)
                {
                    item.Enabled = false;
                }

                enemySpawner.Active = false;
                foreach (Component item in enemySpawner.Components)
                {
                    item.Enabled = false;
                }

                map.Active = false;
                foreach (Component item in map.Components)
                {
                    item.Enabled = false;
                }
            }

            public IState OnStateUpdate()
            {
                for (int i = 0; i < bomberMans.Count; i++)
                {
                    if (bomberMans[i].Active == false)
                        timer.Update(false);
                }
                Console.WriteLine("Multi");

                Node.ShowPath();

                Console.WriteLine(timer.currentTime);
                if (timer.IsOver())
                {
                    timer.Stop(true);
                    OnStateExit();
                    NextLose.OnStateEnter();
                    return NextLose;
                }

                return this;
            }

            private void LoadLevels()
            {
                if (map == null)
                {
                    LevelManager.Add("Levels/Level00.csv");
                }
                else
                {
                    map.Active = true;
                    foreach (Component item in map.Components)
                    {
                        item.Enabled = false;
                    }
                }
            }

            private void LoadGameObjects()
            {


            }
        }

        private class WinState : IState
        {
            private GameManager owner { get; set; }

            public WinState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                throw new NotImplementedException();
            }

            public void OnStateExit()
            {
                throw new NotImplementedException();
            }

            public IState OnStateUpdate()
            {
                throw new NotImplementedException();
            }
        }

        private class LoseState : IState
        {
            public MenuState Retry { get; set; }

            private GameManager owner { get; set; }
            private MenuBackground loseBg;

            public LoseState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                if (loseBg == null)
                {
                    loseBg = new MenuBackground("Lose");
                    GameObject.Spawn(loseBg);
                }
                else
                {
                    loseBg.Active = true;
                    foreach (Component item in loseBg.Components)
                    {
                        item.Enabled = true;
                    }
                }

            }

            public void OnStateExit()
            {
                loseBg.Active = false;
                foreach (Component item in loseBg.Components)
                {
                    item.Enabled = false;
                }
            }

            public IState OnStateUpdate()
            {
                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Space))
                {
                    OnStateExit();
                    Retry.OnStateEnter();
                    return Retry;
                }

                return this;
            }
        }

        private class PauseState : IState
        {
            public SinglePlayer Next { get; set; }

            private GameManager owner { get; set; }

            public PauseState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                throw new NotImplementedException();
            }

            public void OnStateExit()
            {
                throw new NotImplementedException();
            }

            public IState OnStateUpdate()
            {
                throw new NotImplementedException();
            }
        }
    }
}