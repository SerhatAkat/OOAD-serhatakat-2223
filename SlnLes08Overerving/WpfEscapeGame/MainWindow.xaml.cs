using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room currentRoom;
        Room previousRoom;
        Room bedroom;
        Room _void;
        Item largeKey = new Item(
               "large key",
               "A large key. Could this be my way out? ");

        // event handlers
        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
        }
        public MainWindow()
        {
            InitializeComponent();


            // Define rooms
            bedroom = new Room(
                "bedroom",
                "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, and a bed to the right. ", "/images/ss-bedroom.png");

            Room livingRoom = new Room(
                "living room",
                "A cozy living room with a sofa and a TV.", "/images/ss-living.png");

            Room computerRoom = new Room(
                "computer room",
                "A small room with a computer and a desk.", "/images/ss-computer.png");

            _void = new Room(
                "mysterious room",
                "A big room that is completely empty.", null);

            // Define items
            Item smallKey = new Item(
               "small silver key",
               "A small silver key, makes me think of one I had at highschool.");

            Item bed = new Item(
               "bed",
               "Just a bed. I am not tired right now. ", false);
            bed.HiddenItem = smallKey;

            LockableItem locker = new LockableItem(
                "locker",
                "A locker. I wonder what's inside. ",
                true,
                smallKey);


            locker.HiddenItem = largeKey;
            locker.IsLocked = true;
            locker.Key = smallKey;

            Item floorMat = new Item(
               "floor mat",
               "A bit ragged floor mat, but still one of the most popular designs. ");


            // item stoel en poster
            Item stoel = new Item("stoel", "Een gewone houten stoel. Lijkt niet erg comfortabel.", false);
            Item poster = new Item("poster", "Een poster aan de muur. Het lijkt op een filmadvertentie.", true);

            // setup bedroom
            bedroom.Items.Add(new Item(
               "floor mat",
               "A bit ragged floor mat, but still one of the most popular designs. "));
            bedroom.Items.Add(bed);
            bedroom.Items.Add(locker);
            bedroom.Items.Add(stoel);
            bedroom.Items.Add(poster);
            bedroom.Items.Add(floorMat);
            bedroom.Items.Add(smallKey);
            bedroom.Items.Add(largeKey);

            // setup living room
            livingRoom.Items.Add(new Item("floor mat", "A cozy floor mat that complements the decor."));
            livingRoom.Items.Add(new Item("poster", "A poster of a beach scene. Makes me want to go on vacation."));
            livingRoom.Items.Add(new Item("TV", "A tv, looks like it's broken.", false));


            // setup computer room
            computerRoom.Items.Add(new Item("floor mat", "A floor mat with a geeky design."));
            computerRoom.Items.Add(new Item("poster", "A poster of a famous computer scientist. He's my hero."));
            computerRoom.Items.Add(new Item("computer", "A computer, looks like it doesn't work.", false));


            // 2. Add four doors
            Door door1 = new Door(
                "living door",
                true, // isLocked
                largeKey, // key
                livingRoom);

            Door door2 = new Door(
                "computer door",
                false, // isLocked
                null, // key
                computerRoom);

            Door door3 = new Door(
                "mysterious door",
                false, // isLocked
                null, // key
                _void); // null ruimte

            Door door4 = new Door(
                "bedroom door",
                true, // isLocked
                largeKey, // key
                bedroom); // null ruimte


            // Set the keys for doors 1 and 4
            door1.Key = largeKey;

            // Add doors to the rooms
            livingRoom.Doors.Add(door2);
            livingRoom.Doors.Add(door3);
            livingRoom.Doors.Add(door4);
            computerRoom.Doors.Add(door1);
            bedroom.Doors.Add(door1);
            _void.Doors.Add(door1);

            // start game
            currentRoom = bedroom;
            lblMessage.Content = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night... ";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }
        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            lstRoomDoors.Items.Clear();

            // Voeg items toe aan de lstRoomItems ListBox
            foreach (Item itm in currentRoom.Items)
            {
                lstRoomItems.Items.Add(itm);
            }

            // Voeg deuren toe aan de lstRoomDoors ListBox
            foreach (Door door in currentRoom.Doors)
            {
                lstRoomDoors.Items.Add(door);
            }

            // Voeg deuren toe die naar de huidige kamer leiden, behalve de deur naar de huidige kamer zelf
            foreach (Room room in currentRoom.FindConnectedRooms())
            {
                foreach (Door door in room.Doors)
                {
                    if (door.LeadsTo == currentRoom)
                    {
                        continue; // Skip this door, it's the one leading to the current room
                    }

                    if (currentRoom.Doors.Contains(door))
                    {
                        continue; // Skip this door, it's already in the list
                    }

                    lstRoomDoors.Items.Add(door);
                }
            }

            // Verwijder items uit de lijst van de vorige kamer die niet in de nieuwe kamer zijn
            if (previousRoom != null && previousRoom != currentRoom)
            {
                foreach (Item itm in previousRoom.Items.Where(itm => !currentRoom.Items.Contains(itm)))
                {
                    lstRoomItems.Items.Remove(itm);
                }
            }

            // Sla de huidige kamer op als de vorige kamer voor de volgende keer dat deze methode wordt aangeroepen
            previousRoom = currentRoom;

            // Update the room image
            SetRoomImage();
        }





        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            LockableItem roomItem = (LockableItem)lstRoomItems.SelectedItem;
            // 2. is it locked?
            if (roomItem.IsLocked)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }
            // 3. does it contain a hidden item?
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
                lstMyItems.Items.Add(foundItem);
                roomItem.HiddenItem = null;
                return;
            }
            // 4. just another item; show description
            lblMessage.Content = roomItem.Description;
        }

        private void BtnUseOn_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            LockableItem roomItem = (LockableItem)lstRoomItems.SelectedItem;

            // 2. check if items exist and item doesn't fit
            if (myItem == null || roomItem.Key != myItem)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }

            // 3. item fits; other item unlocked
            roomItem.IsLocked = false;
            roomItem.Key = null;
            lstMyItems.Items.Remove(myItem);
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
        }



        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;

            if (!selItem.IsPortable)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }

            // 2. add item to your items list
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);
            btnDrop.IsEnabled = lstMyItems.Items.Count > 0;
        }
        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (lstMyItems.SelectedItem != null)
            {
                Item selItem = (Item)lstMyItems.SelectedItem;
                lblMessage.Content = $"I just dropped the {selItem.Name}. ";
                lstMyItems.Items.Remove(selItem);
                lstRoomItems.Items.Add(selItem);
                currentRoom.Items.Add(selItem);
                btnDrop.IsEnabled = lstMyItems.Items.Count > 0;
            }
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (lstRoomDoors.SelectedItem is Door door)
            {
                if (door.IsLocked)
                {
                    // 1. Find the key required to open the door.
                    Item requiredKey = door.Key;

                    // 2. Check if the player has the required key.
                    if (lstMyItems.Items.Contains(requiredKey))
                    {

                        // 3. Unlock the door.
                        door.IsLocked = false;
                        lblMessage.Content = $"You unlocked the {door.Name} with the {requiredKey.Name}.";

                        // 4. Go to the next room.
                        currentRoom = door.LeadsTo;
                        txtRoomDesc.Text = currentRoom.Description;
                        UpdateUI();
                        lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
                    }
                    else
                    {
                        lblMessage.Content = $"The {door.Name} is locked. You need a {requiredKey.Name} to open it.";
                    }
                }
                else
                {
                    currentRoom = door.LeadsTo;
                    txtRoomDesc.Text = currentRoom.Description;
                    UpdateUI();
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
                }
            }
            else
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
            }
        }

        private void BtnOpenWith_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button clicked");
            if (lstRoomDoors.SelectedItem is Door door && lstMyItems.SelectedItem is Item myItem)
            {
                if (door.IsLocked && door.Key == myItem)
                {
                    door.IsLocked = false;
                    door.Key = null;
                    lblMessage.Content = $"You unlocked the {door.Name} with the {myItem.Name}.";
                }
                else
                {
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                }
            }
        }
        // afbeeldingen
        private void SetRoomImage()
        {
            if (currentRoom.ImagePath != null)
            {
                imgFoto.Source = new BitmapImage(new Uri(currentRoom.ImagePath, UriKind.Relative));
            }
            else
            {
                imgFoto.Source = null; // Clear the image control if there's no image path
            }
        }







    }
}
