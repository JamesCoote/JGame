using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class logic : MonoBehaviour {


	//----------------------------------------------------//
	#region constants
	//----------------------------------------------------//
	private const int numPlanetsPerSystem = 12;
	private const long updateInterval = 1; // in seconds
	private const float secondsInMinute = 60.0f; // change this to speed up / slow down the game. higher values = slower, lower values = faster
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//
	#region data
	//----------------------------------------------------//
	public static List<Resource> baseResources = new List<Resource>();
	public static Dictionary<string, Resource> resourcesNameMap = new Dictionary<string, Resource>();
	public static List<Building> baseAllBuildings = new List<Building>();
	public static List<ResourceBuilding> baseResourceBuildings = new List<ResourceBuilding>();
	public static List<FacilitiesBuilding> baseFacilitiesBuildings = new List<FacilitiesBuilding>();
	public static List<Technology> baseTechnologies = new List<Technology>();
	public static List<ShipType> baseShipTypes = new List<ShipType>();
	public List<SolarSystem> systems = new List<SolarSystem>();
	public Player player;
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//
	private int ticks = 1;
	//----------------------------------------------------//
	void Start () {
		// initialise
		createResources();
		createUniverse();

		// set initial UI view
		mid = uiOverview;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.fixedTime / updateInterval > ticks){
			ticks++;
			// update
			foreach(Planet planet in player.planets){
				planet.Update();
			}
		}

		if(uiTakeKeyboardInput!=null){
			uiTakeKeyboardInput.Invoke();
		}
	}
	//----------------------------------------------------//
	#region initialisation
	//----------------------------------------------------//
	// Use this section to initialise data, add new resources, ships, buildings etc
	//----------------------------------------------------//
	private void createResources(){
		Resource metal = new Resource();
		metal.name = "Metal";
		metal.texture = (Texture2D)Resources.Load("trilithene_large_notext");
		baseResources.Add(metal);
		resourcesNameMap.Add(metal.name, metal);

		Resource crystal = new Resource();
		crystal.name = "Crystal";
		crystal.texture = (Texture2D)Resources.Load("azurite_large_notext");
		baseResources.Add(crystal);
		resourcesNameMap.Add(crystal.name, crystal);

		Resource deuterium = new Resource();
		deuterium.name = "Deuterium";
		deuterium.texture = (Texture2D)Resources.Load("neptunite_large_notext");
		baseResources.Add(deuterium);
		resourcesNameMap.Add(deuterium.name, deuterium);


		// create buildings

		// resource buildings (mines)
		ResourceBuilding metalMine = new ResourceBuilding();
		metalMine.name = "Metal Mine";
		metalMine.energy = -5;
		metalMine.resource = metal;
		metalMine.baseAmount = 10;
		metalMine.baseTime = 30;
		metalMine.levelTimeMultiplier = 1.2f;
		metalMine.levelEnergyMultiplier = 1.2f;
		metalMine.levelResourceMultiplier = 1.1f;
		metalMine.levelCostMultiplier = 1.5f;
		metalMine.cost.Add(metal, 45);
		metalMine.cost.Add(crystal, 15);
		metalMine.icon = (Texture2D)Resources.Load("mine_1");

		baseAllBuildings.Add(metalMine);
		baseResourceBuildings.Add(metalMine);

		ResourceBuilding crystalMine = new ResourceBuilding();
		crystalMine.name = "Crystal Mine";
		crystalMine.energy = -7;
		crystalMine.resource = crystal;
		crystalMine.baseAmount = 6;
		crystalMine.baseTime = 40;
		crystalMine.levelTimeMultiplier = 1.3f;
		crystalMine.levelEnergyMultiplier = 1.2f;
		crystalMine.levelResourceMultiplier = 1.1f;
		crystalMine.levelCostMultiplier = 1.8f;
		crystalMine.cost.Add(metal, 50);
		crystalMine.cost.Add(crystal, 25);
		crystalMine.icon = (Texture2D)Resources.Load("mine_2");

		baseAllBuildings.Add(crystalMine);
		baseResourceBuildings.Add(crystalMine);

		ResourceBuilding deuteriumSynthesiser = new ResourceBuilding();
		deuteriumSynthesiser.name = "Deuterium Synthesiser";
		deuteriumSynthesiser.energy = -10;
		deuteriumSynthesiser.resource = deuterium;
		deuteriumSynthesiser.baseAmount = 4;
		deuteriumSynthesiser.baseTime = 45;
		deuteriumSynthesiser.levelTimeMultiplier = 1.45f;
		deuteriumSynthesiser.levelEnergyMultiplier = 1.5f;
		deuteriumSynthesiser.levelResourceMultiplier = 1.1f;
		deuteriumSynthesiser.levelCostMultiplier = 1.6f;
		deuteriumSynthesiser.cost.Add(metal, 40);
		deuteriumSynthesiser.cost.Add(crystal, 35);
		deuteriumSynthesiser.icon = (Texture2D)Resources.Load("deuterium_1");

		baseAllBuildings.Add(deuteriumSynthesiser);
		baseResourceBuildings.Add(deuteriumSynthesiser);

		Building solarPlant = new Building();
		solarPlant.name = "Solar Plant";
		solarPlant.energy = 12;
		solarPlant.baseTime = 35;
		solarPlant.levelTimeMultiplier = 1.25f;
		solarPlant.levelEnergyMultiplier = 1.45f;
		solarPlant.cost.Add(metal, 60);
		solarPlant.cost.Add(crystal, 30);
		solarPlant.levelCostMultiplier = 1.6f;
		solarPlant.icon = (Texture2D)Resources.Load("solar_1");

		baseAllBuildings.Add(solarPlant);

		// facilities buildings
		FacilitiesBuilding roboticsFactory = new FacilitiesBuilding();
		roboticsFactory.name = "Robotics Factory";
		roboticsFactory.baseTime = 60;
		roboticsFactory.levelTimeMultiplier = 1.25f;
		roboticsFactory.cost.Add(metal,200);
		roboticsFactory.cost.Add(crystal,50);
		roboticsFactory.cost.Add(deuterium,75);
		roboticsFactory.levelCostMultiplier = 1.5f;
		roboticsFactory.icon = (Texture2D)Resources.Load("robotics_1");

		baseAllBuildings.Add(roboticsFactory);
		baseFacilitiesBuildings.Add(roboticsFactory);

		FacilitiesBuilding shipyard = new FacilitiesBuilding();
		shipyard.name = "Shipyard";
		shipyard.baseTime = 70;
		shipyard.levelTimeMultiplier = 1.35f;
		shipyard.cost.Add(metal,400);
		shipyard.cost.Add(crystal,250);
		shipyard.cost.Add(deuterium,125);
		shipyard.prequisites.Add(roboticsFactory, 2);
		shipyard.levelCostMultiplier = 1.5f;
		shipyard.icon = (Texture2D)Resources.Load("shipyard_1");

		baseAllBuildings.Add(shipyard);
		baseFacilitiesBuildings.Add(shipyard);

		FacilitiesBuilding researchLab = new FacilitiesBuilding();
		researchLab.name = "Research Lab";
		researchLab.baseTime = 45;
		researchLab.levelTimeMultiplier = 1.5f;
		researchLab.cost.Add(metal, 150);
		researchLab.cost.Add(crystal, 500);
		researchLab.cost.Add(deuterium, 250);
		researchLab.levelCostMultiplier = 1.5f;
		researchLab.icon = (Texture2D)Resources.Load("research_1");

		baseAllBuildings.Add(researchLab);
		baseFacilitiesBuildings.Add(researchLab);

		// technologies
		Technology energyTechnology = new Technology();
		energyTechnology.name = "Energy Technology";
		energyTechnology.baseTime = 120;
		energyTechnology.levelTimeMultiplier = 1.75f;
		energyTechnology.cost.Add(crystal,500);
		energyTechnology.cost.Add(deuterium,250);
		energyTechnology.levelCostMultiplier = 1.5f;
		energyTechnology.prequisites.Add(researchLab,1);
		energyTechnology.icon = (Texture2D)Resources.Load("energy_1");

		baseTechnologies.Add(energyTechnology);

		Technology combustionDrive = new Technology();
		combustionDrive.name = "Combustion Drive";
		combustionDrive.baseTime = 175;
		combustionDrive.levelTimeMultiplier = 2.0f;
		combustionDrive.cost.Add(metal,350);
		combustionDrive.cost.Add(deuterium,750);
		combustionDrive.prequisites.Add(energyTechnology,1);
		combustionDrive.levelCostMultiplier = 1.5f;
		combustionDrive.icon = (Texture2D)Resources.Load("combustion_drive_1");

		baseTechnologies.Add(combustionDrive);

		// ship types
		ShipType lightFighter = new ShipType();
		lightFighter.name = "Light Fighter";
		lightFighter.baseTime = 200;
		lightFighter.levelTimeMultiplier = 1.0f;
		lightFighter.cost.Add(metal,1000);
		lightFighter.cost.Add(crystal,750);
		lightFighter.levelCostMultiplier = 1.0f;
		lightFighter.prequisites.Add(combustionDrive,1);
		lightFighter.prequisites.Add(shipyard, 2);

		baseShipTypes.Add(lightFighter);

	}
	//----------------------------------------------------//
	private void createUniverse(){
		// create player
		player = new Player();
		player.name = "James";

		// create solarsystem
		SolarSystem homeSystem = new SolarSystem();

		// populate it with planets
		for(int i = 0;i<numPlanetsPerSystem;i++){
			homeSystem.planets.Add(new Planet(homeSystem));
		}

		systems.Add(homeSystem);

		// create player and give them a starting planet
		Planet startPlanet = homeSystem.planets[Random.Range(0,homeSystem.planets.Count-1)];
		player.planets.Add(startPlanet);
		startPlanet.owner = player;
		startPlanet.name = "Homeworld";
		player.currentPlanet = startPlanet;

		// starting resources
		player.currentPlanet.resources[resourcesNameMap["Metal"]] = 600;
		player.currentPlanet.resources[resourcesNameMap["Crystal"]] = 500;
		player.currentPlanet.resources[resourcesNameMap["Deuterium"]] = 0;
		//player.currentPlanet.resources[resourcesNameMap["Metal"]] = 5000;
		//player.currentPlanet.resources[resourcesNameMap["Crystal"]] = 5000;
		//player.currentPlanet.resources[resourcesNameMap["Deuterium"]] = 5000;
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region UI
	//----------------------------------------------------//
	// The game UI is generated entirely at runtime using Unity.GUI functions
	// All of which are in this section
	//----------------------------------------------------//


	//----------------------------------------------------//
	#region UI members
	//----------------------------------------------------//
	/// <summary>
	/// delegate used for determining which UI view to show in main view
	/// </summary>
	private delegate void uiMid();

	/// delegate used for determining which UI view to show in main view
	uiMid mid;
	
	private int energyPercentSelector = -1; // see energy management in uiResources()

	/// currently selected object
	private LevelObject objSelected;

	/// current info being edited (see uiEditSelected())
	private int editSelectedCategory = 0;
	private int addInEditSelect = -1;

	/// Where data being entered by user is stored
	private string dataEntered = "";
	/// use this to make sure you don't accidentally take data when you shouldn't be
	private int dataEntryBelongsToView = 0; 
	private delegate void uiDataEntry();
	uiDataEntry uiTakeKeyboardInput;

	// style information
	private GUIStyle resourceBoxStyle;
	private GUIStyle energyManagementStyle;
	private GUIStyle titleStyle;
	private GUIStyle subtitleStyle;
	private GUIStyle rightAlignStyle;
	private Texture energyTexture;
	private Texture planetOverviewTexture;
	private Texture resourceGeneralTexture;
	private Texture researchGeneralTexture;
	private Texture facilitiesGeneralTexture;
	private Texture shipyardGeneralTexture;

	// constants
	private const int UI_TOP_RESOURCES_Y = 80;
	private const int UI_RESOURCEBUTTON_WIDTH = 50;
	private const int UI_MENU_WIDTH = 150;
	private const int UI_MAIN_WIDTH = 500;
	private const int UI_MAIN_HALFHEIGHT = 250;
	private const int UI_HEADING_HEIGHT = 35;
	private const int UI_HEADING_TOPPADDING = 10;
	private const int UI_IMG_PADDING = 10;
	private const int UI_BOX_WIDTH = 75;
	private const int UI_BOX_IMG_PADDING = 5;
	private const int UI_BOX_LABEL_HEIGHT = 25;
	private const int UI_ACTIONBUTTON_HEIGHT = 50;
	private const int UI_SELECTBOX_WIDTH = 180;
	private const int UI_EDIT_WIDTH = 300;

	private const int UI_ID_RESOURCE_VIEW = 1;
	private const int UI_ID_EDIT_VIEW_NAME = 2;
	private const int UI_ID_EDIT_VIEW_BASE_TIME = 3;
	private const int UI_ID_EDIT_VIEW_TIME_MULTIPLIER = 4;
	private const int UI_ID_EDIT_VIEW_ENERGY = 5;
	private const int UI_ID_EDIT_VIEW_ENERGY_MULTIPLIER = 6;
	private const int UI_ID_EDIT_VIEW_BASE_COST = 7;
	private const int UI_ID_EDIT_VIEW_COST_MULTIPLIER = 8;
	private const int UI_ID_EDIT_VIEW_PRODUCTION = 9;
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region UI Initialisation
	//----------------------------------------------------//
	// Initialising the UI
	//----------------------------------------------------//
	void OnGUI () {

		// init styles first time
		if(resourceBoxStyle==null){
			initGUIStyles();
		}

		// draw menu and resource numbers across top
		uiMenu();
		uiTopResources();

		// draw box to edit selected object on right hand side
		uiEditSelected();

		// draw whatever in main view
		mid.Invoke();
	}
	//----------------------------------------------------//
	public void initGUIStyles(){
		GUI.skin.GetStyle("Label").normal.textColor = new Color(229.0f/255.0f,204.0f/255.0f,221.0f/255.0f);
		GUI.skin.GetStyle("Button").normal.textColor = new Color(229.0f/255.0f,204.0f/255.0f,221.0f/255.0f);

		resourceBoxStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		resourceBoxStyle.alignment = TextAnchor.UpperCenter;

		energyManagementStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		energyManagementStyle.alignment = TextAnchor.MiddleLeft;

		titleStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		titleStyle.alignment = TextAnchor.UpperCenter;
		titleStyle.fontSize = 25;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.normal.textColor = new Color(253.0f/255.0f,187.0f/255.0f,232.0f/255.0f);

		rightAlignStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		rightAlignStyle.alignment = TextAnchor.MiddleRight;

		subtitleStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		subtitleStyle.fontStyle = FontStyle.Bold;
		subtitleStyle.normal.textColor = new Color(146.0f/255.0f,106.0f/255.0f,212.0f/255.0f);

		energyTexture = (Texture2D)Resources.Load("vanadite_large_notext");
		planetOverviewTexture = (Texture2D)Resources.Load("art_planet");
		resourceGeneralTexture = (Texture2D)Resources.Load("resources_background");
		researchGeneralTexture = (Texture2D)Resources.Load("research_background");
		facilitiesGeneralTexture = (Texture2D)Resources.Load("facilities_background");
		shipyardGeneralTexture = (Texture2D)Resources.Load("shipyard_background");
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region UI always visible
	//----------------------------------------------------//

	/// <summary>
	/// draws resource amounts player owns across top of screen
	/// </summary>
	public void uiTopResources(){

		int topPadding = 10;
		int horizontalPadding = 120;
		//int horizontalPadding = (Screen.width - ((baseResources.Count+1) * buttonSize)) / (baseResources.Count+2);
		int currentX = horizontalPadding;
		int currentY = topPadding;

		// loop through resources and draw
		Dictionary<Resource,int> currentResources = player.currentPlanet.resources;		
		foreach(KeyValuePair<Resource,int> entry in currentResources)
		{
			uiResourceIcon(entry.Key, entry.Value, currentX, currentY);
			currentX += horizontalPadding;
		}

		// draw amount of energy as well
		uiResourceIcon(energyTexture, getEnergyBalance(player.currentPlanet).ToString(), currentX, currentY);
	}
	//----------------------------------------------------//

	/// <summary>
	/// main / left hand side menu
	/// </summary>	 
	public void uiMenu(){

		int topPadding = 10;
		int bottomPadding = 10;
		int horizontalPadding = 5;
		int interButtonPadding = 5;
		int buttonHeight = 50;
		int numButtons = 6;
		int currentX = 0;
		int currentY = UI_TOP_RESOURCES_Y + topPadding;

		// draw background box
		GUI.Box (new Rect(currentX,UI_TOP_RESOURCES_Y,UI_MENU_WIDTH,
		                  (numButtons * buttonHeight) + ((numButtons-1) * interButtonPadding) + topPadding + bottomPadding),"");

		currentX += horizontalPadding;
		currentY += topPadding;

		// list buttons
		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Overview")){
			mid = uiOverview;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Resources")){
			mid = uiResources;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Facilities")){
			mid = uiFacilities;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Research")){
			mid = uiResearch;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Shipyard")){
			mid = uiShipyard;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "1";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		if (GUI.Button (new Rect (currentX,currentY,UI_MENU_WIDTH - (horizontalPadding * 2),buttonHeight), "Galaxy")){
			mid = uiGalaxy;
			objSelected = null;
			dataEntryBelongsToView = 0;
			uiTakeKeyboardInput = null;
			dataEntered = "";
			energyPercentSelector = -1;
		}
		currentY += buttonHeight + interButtonPadding;

		// NOTE: be sure to make sure numButtons is correct when you add more buttons!

	}
	//----------------------------------------------------//

	/// <summary>
	/// List of planets owned by the player
	/// </summary>
	public void uiPlanets(){
		// todo
	}
	//----------------------------------------------------//

	/// <summary>
	/// Edit the selected item
	/// </summary>
	public void uiEditSelected(){
		int currentX  = UI_MENU_WIDTH + UI_MAIN_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;
		int padding = 10;
		int height = 25;

		GUI.Box(new Rect(currentX, currentY, UI_EDIT_WIDTH, UI_MAIN_HALFHEIGHT*2),"");

		currentY += padding;
		currentX += padding;

		if(GUI.Button(new Rect(currentX, currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Time")){
			editSelectedCategory = 1;
		}
		if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Produce/Consume")){
			editSelectedCategory = 2;
		}

		currentY += height;
		currentX = UI_MENU_WIDTH + UI_MAIN_WIDTH + padding;

		if(GUI.Button(new Rect(currentX, currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Cost")){
			editSelectedCategory = 3;
		}
		if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Prerequisites")){
			editSelectedCategory = 4;
		}

		currentY += height;
		currentX = UI_MENU_WIDTH + UI_MAIN_WIDTH;

		if(objSelected!=null){

			currentY += padding;
			currentX += padding;

			uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiStringInput, "Name: ", objSelected.name, UI_ID_EDIT_VIEW_NAME);
			if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_NAME){
				objSelected.name = dataEntered;
			}

			int tempY = 0;
			int count = -1;

			switch(editSelectedCategory){
				// time
				case 1:
				// base time
				uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiPositiveIntegerInput, "Base Build Time: ", objSelected.baseTime.ToString(), UI_ID_EDIT_VIEW_BASE_TIME);
				if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_BASE_TIME){
					if(!int.TryParse(dataEntered, out objSelected.baseTime)){
						objSelected.baseTime = 0;
					}
				}

				// time multiplier
				uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiDecimalInput, "Level vs Build Time: ", objSelected.levelTimeMultiplier.ToString(), UI_ID_EDIT_VIEW_TIME_MULTIPLIER);
				if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_TIME_MULTIPLIER){
					if(!float.TryParse(dataEntered, out objSelected.levelTimeMultiplier)){
						objSelected.levelTimeMultiplier = 0.0f;
					}
				}
				break;

			case 2:
				// produce/consume

				// resource production / consumption

				if(objSelected.GetType()==typeof(ResourceBuilding)){
					// add production/consumption
					tempY = currentY;
					currentY -= height*2 + 10;
					if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Set Product")){
						if(addInEditSelect>0){
							addInEditSelect = -1;
						} else {
							addInEditSelect = 2;
						}
					}
					currentY += height;
					
					// add cost selection buttons
					if(addInEditSelect==2){
						int tempY2 = currentY;
						foreach(Resource resource in baseResources){
							if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height),resource.name)){
								((ResourceBuilding)objSelected).resource = resource;
								addInEditSelect = -1;
							}
							currentY+=height;
						}
						currentY = tempY2;
					}

					currentY = tempY;
										
					// display existing production

					Resource res = ((ResourceBuilding)objSelected).resource;
					// resource name label
					GUI.Label(new Rect(currentX, currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), res.name + ":");
					// remove cost button
					/*
					if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY+height, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Remove")){
						((ResourceBuilding)objSelected).resource = null;
					}*/
					// change amount button
					uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiPositiveIntegerInput, "", ((ResourceBuilding)objSelected).baseAmount.ToString(), UI_ID_EDIT_VIEW_PRODUCTION);
					if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_PRODUCTION){
						if(!int.TryParse(dataEntered, out ((ResourceBuilding)objSelected).baseAmount)){
							((ResourceBuilding)objSelected).baseAmount = 0;
						}
					}
				}


				// energy
				if(objSelected.GetType()==typeof(Building) || objSelected.GetType().IsSubclassOf(typeof(Building))){
					tempY = currentY;
					uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiIntegerInput, "Energy: ", ((Building)objSelected).energy.ToString(), UI_ID_EDIT_VIEW_ENERGY);
					if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_ENERGY){
						if(!int.TryParse(dataEntered, out ((Building)objSelected).energy)){
							((Building)objSelected).energy = 0;
						}
					}
					int tempX = currentX;
					currentY = tempY;
					currentX += (UI_EDIT_WIDTH / 2);
					uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiIntegerInput, "Level vs Energy: ", ((Building)objSelected).levelEnergyMultiplier.ToString(), UI_ID_EDIT_VIEW_ENERGY_MULTIPLIER);
					if(dataEntryBelongsToView==UI_ID_EDIT_VIEW_ENERGY_MULTIPLIER){
						if(!float.TryParse(dataEntered, out ((Building)objSelected).levelEnergyMultiplier)){
							((Building)objSelected).levelEnergyMultiplier = 0;
						}
					}
					currentX = tempX;
				}

				break;

			case 3:
				// cost

				// add cost
				tempY = currentY;
				currentY -= height*2 + 10;
				if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Add Cost")){
					if(addInEditSelect>0){
						addInEditSelect = -1;
					} else {
						addInEditSelect = 1;
					}
				}
				currentY += height;

				// add cost selection buttons
				if(addInEditSelect==1){
					int tempY2 = currentY;
					foreach(Resource resource in baseResources){
						if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height),resource.name)){
							if(!objSelected.cost.ContainsKey(resource)){
								objSelected.cost.Add(resource, 0);
								addInEditSelect = -1;
							}
						}
						currentY+=height;
					}
					currentY = tempY2;
				}
				currentY = tempY;

				// loop through existing costs
				count = -1;
				Resource setCostRes = null;
				int nCost = 0;
				foreach(KeyValuePair<Resource, int> entry in objSelected.cost){
					Resource res = entry.Key;
					// resource name label
					GUI.Label(new Rect(currentX, currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), res.name + ":");
					// remove cost button
					if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY+height, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Remove")){
						setCostRes = res;
						nCost = -1;
					}
					// change amount button
					uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiPositiveIntegerInput, "", entry.Value.ToString(), count);
					if(dataEntryBelongsToView==count){
						int cost = 0;
						if(!int.TryParse(dataEntered, out cost)){
							cost = 0;
						}
						setCostRes = res;
						nCost = cost;
					}
					count--;
				}

				// change any resources that need changing (we assume the user can only change one resource at a time)
				if(setCostRes!=null){
					if(nCost>0){
						objSelected.cost[setCostRes] = nCost;
					} else if(nCost<0){
						objSelected.cost.Remove(setCostRes);
					}
				}

				break;

			case 4:
				// prerequisites

				// add prerequisite button
				tempY = currentY;
				currentY -= height*2 + 10;
				if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Add Prerequisite")){
					if(addInEditSelect>0){
						addInEditSelect = -1;
					} else {
						addInEditSelect = 3;
					}
				}
				currentY += height;
				
				// add cost selection buttons
				if(addInEditSelect==3){
					int tempY2 = currentY;
					foreach(Building building in baseAllBuildings){
						if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height),building.name)){
							if(!objSelected.prequisites.ContainsKey(building)){
								objSelected.prequisites.Add(building, 0);
								addInEditSelect = -1;
							}
						}
						currentY+=height;
					}
					foreach(Technology tech in baseTechnologies){
						if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height),tech.name)){
							if(!objSelected.prequisites.ContainsKey(tech)){
								objSelected.prequisites.Add(tech, 0);
								addInEditSelect = -1;
							}
						}
						currentY+=height;
					}
					foreach(ShipType ship in baseShipTypes){
						if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height),ship.name)){
							if(!objSelected.prequisites.ContainsKey(ship)){
								objSelected.prequisites.Add(ship, 0);
								addInEditSelect = -1;
							}
						}
						currentY+=height;
					}
					currentY = tempY2;
				}
				currentY = tempY;
				
				// loop through existing prerequisites
				count = -1;
				LevelObject setPrereq = null;
				int nLevel = 0;
				foreach(KeyValuePair<LevelObject, int> entry in objSelected.prequisites){
					LevelObject obj = entry.Key;
					// resource name label
					GUI.Label(new Rect(currentX, currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height), obj.name + ":");
					// remove cost button
					if(GUI.Button(new Rect(currentX + (UI_EDIT_WIDTH / 2), currentY+height, (UI_EDIT_WIDTH / 2) - (padding*2), height), "Remove")){
						setPrereq = obj;
						nLevel = -1;
					}
					// change amount button
					uiKeyboardButton(ref currentX, ref currentY, (UI_EDIT_WIDTH / 2) - (padding*2), height,padding,uiPositiveIntegerInput, "", entry.Value.ToString(), count);
					if(dataEntryBelongsToView==count){
						int level = 0;
						if(!int.TryParse(dataEntered, out level)){
							level = 0;
						}
						setPrereq = obj;
						nLevel = level;
					}
					count--;
				}
				
				// change any resources that need changing (we assume the user can only change one resource at a time)
				if(setPrereq!=null){
					if(nLevel>0){
						objSelected.prequisites[setPrereq] = nLevel;
					} else if(nLevel<0){
						objSelected.prequisites.Remove(setPrereq);
					}
				}


				break;
			default:
				break;
			}

		} else {
			GUI.Label(new Rect(currentX + padding, currentY + padding, UI_EDIT_WIDTH, height), "(Select an item to edit it)",resourceBoxStyle);
		}

	}

	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region UI main view
	//----------------------------------------------------//
	// Methods for drawing the main view (view in the centre
	// of the screen)
	//----------------------------------------------------//

	/// <summary>
	/// overview of the planet selected
	/// </summary>
	public void uiOverview(){

		int vPadding = 10;

		// background box
		GUI.Box(new Rect(UI_MENU_WIDTH,UI_TOP_RESOURCES_Y,UI_MAIN_WIDTH,UI_MAIN_HALFHEIGHT),"");

		// background image
		uiBackground(UI_MENU_WIDTH, UI_TOP_RESOURCES_Y, UI_MAIN_WIDTH, UI_MAIN_HALFHEIGHT, "Overview - " + player.currentPlanet.name, planetOverviewTexture, 1);

		int currentY = UI_TOP_RESOURCES_Y + UI_HEADING_HEIGHT + vPadding;

		// details of planet take up right hand side of overview.
		int detailsWidth = UI_MAIN_WIDTH / 2;
		int detailsPaddingRight = 15;
		int detailsInfoWidth = (detailsWidth / 2) - detailsPaddingRight;
		int detailsHeight = 25;

		// planet size
		GUI.Label(new Rect(UI_MENU_WIDTH + UI_MAIN_WIDTH - detailsWidth,
		                   currentY,
		                   detailsWidth - detailsInfoWidth,
		                   detailsHeight),"Diameter: ", subtitleStyle);
		GUI.Label(new Rect(UI_MENU_WIDTH + UI_MAIN_WIDTH - detailsWidth + detailsInfoWidth,
		                   currentY,
		                   detailsInfoWidth,
		                   detailsHeight),player.currentPlanet.diameter.ToString() + " km", rightAlignStyle);
		currentY += detailsHeight + vPadding;

		// planet temperature
		GUI.Label(new Rect(UI_MENU_WIDTH + UI_MAIN_WIDTH - detailsWidth,
		                   currentY,
		                   detailsWidth - detailsInfoWidth,
		                   detailsHeight),"Temperature: ", subtitleStyle);
		GUI.Label(new Rect(UI_MENU_WIDTH + UI_MAIN_WIDTH - detailsWidth + detailsInfoWidth,
		                   currentY,
		                   detailsInfoWidth,
		                   detailsHeight),player.currentPlanet.minTemperature.ToString() + "\u00B0C to " + player.planets[0].maxTemperature.ToString()+ "\u00B0C",rightAlignStyle);
		currentY += detailsHeight + vPadding;

	}
	//----------------------------------------------------//

	/// <summary>
	/// view where players build resource buildings and manage energy levels
	/// </summary>
	public void uiResources(){

		int currentX = UI_MENU_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;
		int energyManagerWidth = 200;

		uiLevelObjectDetail<Building>(ref currentX, ref currentY, objSelected, "Resources", "Improve", resourceGeneralTexture, true, false, baseAllBuildings, energyManagerWidth);

		// energy management
		currentY = UI_TOP_RESOURCES_Y + UI_MAIN_HALFHEIGHT;
		currentX = UI_MENU_WIDTH + UI_MAIN_WIDTH - energyManagerWidth;
		int energyManagertextHeight = 25;
		int energyManagertextHeightSmall = 20;
		int energyManagerButtonWidth = 50;
		int padding = 10;

		GUI.Box(new Rect(currentX, currentY,energyManagerWidth,UI_MAIN_HALFHEIGHT),"");
		currentX +=padding;
		GUI.Label(new Rect(currentX, currentY,energyManagerWidth,energyManagertextHeight), "Energy Management", subtitleStyle);
		currentY += energyManagertextHeight + padding;
		int count = 0;

		// draw energy needs and production amounts for each resource building
		foreach(KeyValuePair<Building, int> entry in player.currentPlanet.buildingsLevels){
			Building building = entry.Key;
			if(building.GetType()==typeof(ResourceBuilding)){
				ResourceBuilding resBuilding = (ResourceBuilding)building;

				// building name
				GUI.Label(new Rect(currentX, currentY,energyManagerWidth,energyManagertextHeight), resBuilding.name);
				currentY += energyManagertextHeightSmall;
				// calculate power and output current/max
				int actualOutput = getActualOutput(building, player.currentPlanet);
				int energyNeeded = -getEnergyNeeded(building, player.currentPlanet);
				int actualEnergy = (int)(energyNeeded * getActualEnergy(building, player.currentPlanet));
				GUI.Label(new Rect(currentX, currentY,energyManagerWidth,energyManagertextHeightSmall), "Energy Needed " + actualEnergy.ToString() + "/" + energyNeeded.ToString(), energyManagementStyle);
				currentY +=energyManagertextHeightSmall;
				GUI.Label(new Rect(currentX, currentY,energyManagerWidth,energyManagertextHeightSmall), "Current Production: " + actualOutput.ToString(), energyManagementStyle);

				// players can reduce the power the buildings use

				// button to change energy use - brings up a list of options for various energy levels
				if(GUI.Button(new Rect(currentX + energyManagerWidth - energyManagerButtonWidth, 
				                       currentY - (energyManagertextHeightSmall/2),
				                       energyManagerButtonWidth,
				                       energyManagertextHeightSmall), 
				              ((int)(player.currentPlanet.buildingsEnergy[resBuilding]*100)).ToString() + "%")){
					if(energyPercentSelector>-1){
						energyPercentSelector = -1;
					} else {
						energyPercentSelector = count;
					}

				}

				// if selecting energy levels, draw percentage selector buttons (i.e. the 10%, 20%, 30% etc buttons)
				if(energyPercentSelector==count){
					for(int i = 1;i<11;i++){
						if(GUI.Button(new Rect(currentX + energyManagerWidth - energyManagerButtonWidth, 
						                       currentY - (energyManagertextHeightSmall/2) - (energyManagertextHeightSmall*(i)),
						                       energyManagerButtonWidth,
						                       energyManagertextHeightSmall), 
						              (i * 10).ToString() + "%")){
							energyPercentSelector = -1;
							player.currentPlanet.buildingsEnergy[resBuilding] = (((float)i * 10.0f) / 100.0f); // TODO
						}
					}
				}

				currentY += energyManagertextHeightSmall + padding;
			}
			count++;
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// ui view for building facilities (non-resource producing buildings)
	/// </summary>
	public void uiFacilities(){
		
		int currentX = UI_MENU_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;

		uiLevelObjectDetail<FacilitiesBuilding>(ref currentX, ref currentY, objSelected, "Facilities", "Improve", facilitiesGeneralTexture, false, false, baseFacilitiesBuildings, 0);

	}
	//----------------------------------------------------//

	/// <summary>
	/// ui view for researching
	/// </summary>
	public void uiResearch(){

		int currentX = UI_MENU_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;

		uiLevelObjectDetail<Technology>(ref currentX, ref currentY, objSelected, "Research", "Research", researchGeneralTexture, false, false, baseTechnologies, 0);

	}
	//----------------------------------------------------//

	/// <summary>
	/// ui view for shipyard (building ships. The shipyard building itself is a facilities building)
	/// </summary> 
	public void uiShipyard(){
		int currentX = UI_MENU_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;
		int padding = 10;

		uiLevelObjectDetail<ShipType>(ref currentX, ref currentY, objSelected, "Shipyard", "Build", shipyardGeneralTexture, false, true, baseShipTypes, 0);

	}
	//----------------------------------------------------//

	/// <summary>
	/// ui view to show the current solar system (todo: multiple solar systems)
	/// </summary>
	public void uiGalaxy(){
		int currentX = UI_MENU_WIDTH;
		int currentY = UI_TOP_RESOURCES_Y;

		GUI.Label(new Rect(currentX,currentY,100,25), "Solar System:");
		currentY += 25;
		
		GUI.Label(new Rect(currentX,currentY,50,25), "Planet");
		currentX += 50;
		GUI.Label(new Rect(currentX,currentY,100,25), "Name");
		currentX += 100;
		GUI.Label(new Rect(currentX,currentY,100,25), "Player");
		
		currentY += 25;
		currentX = 160;
		
		int count = 0;
		foreach(Planet planet in systems[0].planets){
			GUI.Label(new Rect(currentX,currentY,100,25), (count + 1).ToString());
			currentX += 50;
			
			GUI.Label(new Rect(currentX,currentY,100,25), planet.name);
			currentX += 100;
			
			string owner = "";
			if(planet.owner!=null){
				owner = planet.owner.name;
			}
			
			GUI.Label(new Rect(currentX,currentY,100,25), owner);
			
			count++;

			currentX = 160;
			currentY += 25;
		}
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region ui utils
	//----------------------------------------------------//
	// utility methods for the UI and methods common to
	// multiple UI elements
	//----------------------------------------------------//


	/// <summary>
	/// convenience method for uiResourceIcon
	/// </summary>
	/// <param name="resource">Resource</param>
	/// <param name="amount">Amount of resource</param>
	/// <param name="currentX">X position of top left corner to draw icon</param>
	/// <param name="currentY">Y position of top left corner to draw icon</param>
	private void uiResourceIcon(Resource resource, int amount, int currentX, int currentY){
		uiResourceIcon(resource.texture, amount.ToString(), currentX, currentY);
	}
	//----------------------------------------------------//

	/// <summary>
	/// draws the resource icon with label below (use int.ToString() for label for the resource amount or cost to build something)
	/// </summary>
	/// <param name="texture">Texture to use for icon</param>
	/// <param name="label">Text to go below icon</param>
	/// <param name="currentX">X position of top left corner to draw icon</param>
	/// <param name="currentY">Y position of top left corner to draw icon</param>
	private void uiResourceIcon(Texture texture, string label, int currentX, int currentY){
		int buttonPadding = 2;
		int labelHeight = 20;

		// background box
		GUI.Box(new Rect(currentX, currentY,UI_RESOURCEBUTTON_WIDTH, UI_RESOURCEBUTTON_WIDTH),"");

		// icon
		GUI.DrawTexture(new Rect(currentX+buttonPadding,
		                         currentY+buttonPadding,
		                         UI_RESOURCEBUTTON_WIDTH-(buttonPadding*2),
		                         UI_RESOURCEBUTTON_WIDTH-(buttonPadding*2)),
		                texture);

		// label
		GUI.Label(new Rect(currentX,
		                   currentY + UI_RESOURCEBUTTON_WIDTH,
		                   UI_RESOURCEBUTTON_WIDTH,
		                   labelHeight), 
		          label, resourceBoxStyle);
	}
	//----------------------------------------------------//

	/// <summary>
	/// draw a texture/picture with rounded corners
	/// </summary>
	/// <param name="currentX">X position of top left corner to draw</param>
	/// <param name="currentY">Y position of top left corner to draw</param>
	/// <param name="width">Width of picture</param>
	/// <param name="height">Height of picture</param>
	/// <param name="label">Label to draw inside top centred of picture</param>
	/// <param name="texture">Texture used for picture</param>
	/// <param name="borderSize">Border size. Distance btween edge of picture and edge of rounded corner box drawn underneath (used to give effect of rounded corners). Usually 1 or 2</param>
	private void uiBackground(int currentX, int currentY, int width, int height, string label, Texture texture, int borderSize){

		// border box
		GUI.Box(new Rect(currentX + UI_IMG_PADDING - borderSize, 
		                 currentY + UI_IMG_PADDING - borderSize, 
		                 width - (UI_IMG_PADDING*2) + (borderSize*2), 
		                 height - (UI_IMG_PADDING*2)+ (borderSize*2)),"");

		// picture / texture
		GUI.DrawTexture(new Rect(currentX + UI_IMG_PADDING, 
		                         currentY + UI_IMG_PADDING, 
		                         width - (UI_IMG_PADDING*2), 
		                         height - (UI_IMG_PADDING*2)),
		                texture);

		GUI.Label(new Rect(currentX,
		                   currentY + UI_HEADING_TOPPADDING,
		                   width,UI_HEADING_HEIGHT),label, titleStyle);
	}
	//----------------------------------------------------//

	/// <summary>
	/// draws a main view. The view is split into two halves. Top half shows the currently selected item (or a background picture if nothing is selected). Bottom half lists items (buildings, ships, technologies etc) to be build/researched or whatever
	/// </summary>
	/// <param name="currentX">X position of top left corner to draw</param>
	/// <param name="currentY">Y position of top left corner to draw</param>
	/// <param name="levelObject">Level object currently selected. Use null for no object selected</param>
	/// <param name="label">Label to use if no object selected</param>
	/// <param name="actionButtonLabel">Action button label. E.g. "Build", "Improve", "Research"</param>
	/// <param name="backgroundTexture">Background texture for when no object selected</param>
	/// <param name="energyBalance">If set to <c>true</c> to display energy balance of selected object</param>
	/// <param name="multiple">If set to <c>true</c>, option to build multiple levels at a time will appear. Use this for ships only</param>
	/// <param name="items">List of level objects that can be selected</param>
	/// <param name="rightPadding">Right padding.</param>
	/// <typeparam name="T">The type of LevelObject for the list of possible items to select (e.g. Technology, Building, ShipType<)/typeparam>
	private void uiLevelObjectDetail<T>(ref int currentX, ref int currentY, 
	                                    LevelObject levelObject, string label, string actionButtonLabel, Texture backgroundTexture,
	                                    bool energyBalance, bool multiple, 
	                                    List<T> items, int rightPadding) where T : LevelObject{

		int padding = 10;

		// currently selected item
		GUI.Box(new Rect(currentX, currentY,UI_MAIN_WIDTH,UI_MAIN_HALFHEIGHT),"");
		if(levelObject!=null){

			int detailsWidth = UI_MAIN_WIDTH - (UI_SELECTBOX_WIDTH + (UI_IMG_PADDING*2));
			int detailsHeight = 25;

			// draw big picture of item
			uiBackground(currentX, currentY,UI_SELECTBOX_WIDTH,UI_SELECTBOX_WIDTH,"",levelObject.icon, 2);

			// get item level (number of ships in hangar / at planet for ships)
			int level = levelObject.getLevel(player.currentPlanet);

			// item name
			currentX +=UI_SELECTBOX_WIDTH + (UI_IMG_PADDING*2);
			currentY +=UI_IMG_PADDING;
			GUI.Label(new Rect(currentX, currentY,detailsWidth,detailsHeight),levelObject.name, subtitleStyle);
			currentY += detailsHeight;
			
			currentY += padding; // padding

			// energy balance (how much energy the selected building produces / consumes)
			if(energyBalance){
				GUI.Label(new Rect(currentX, currentY,detailsWidth,detailsHeight),"Energy Balance:" + ((int)(((Building)objSelected).energy * ((Building)objSelected).levelEnergyMultiplier) * (level+1)).ToString());
				currentY += detailsHeight;
			}

			string costString = "Required to improve to level " + (level + 1).ToString();

			// if a type of item where multiple items are constructed at a time (rather than many levels), 
			// then allow user to enter in an amount to build
			// Note: the last parameter is the delegate method used to take input. In this case, it sends input to the "numToBuild" variable, which
			// we use later
			if(multiple){
				uiKeyboardButton(ref currentX, ref currentY, detailsWidth / 2, detailsHeight, padding, uiPositiveIntegerInput, "Number to Build: ", "1", UI_ID_RESOURCE_VIEW);
				if(dataEntryBelongsToView==UI_ID_RESOURCE_VIEW){
					if(!int.TryParse(dataEntered, out level) || level<=0){
						level = 1;
					}
				} else {
					level = 1; // note: this will set build amount back to 1 when you edit another field. Annoying, but can't think of a way to solve
				}
				costString = "Cost to build " + level.ToString();
				level--; // usually we are upgrading to the next level, so costs are calculated on a +1 basis. Minus 1 to offset this
			}

			// List the price
			GUI.Label(new Rect(currentX, currentY,detailsWidth,detailsHeight),costString + ":");
			currentY +=detailsHeight;
				
			foreach(KeyValuePair<Resource,int> entry in levelObject.cost){
				// draw the resource icon / label to represent the price	
				uiResourceIcon(entry.Key, 
				               ((int)(entry.Value * levelObject.levelCostMultiplier * (level +1))), 
				               currentX, 
				               currentY);
					
				currentX += UI_RESOURCEBUTTON_WIDTH + padding;
			}

			currentY += padding; // padding

			// The action button to build/research/whatever it is we're doing to the selected item

			// Display time till can build
			int timeLeft = levelObject.timeLeft(player.currentPlanet);


			// draw the button
			if (GUI.Button (new Rect (UI_MENU_WIDTH + UI_MAIN_WIDTH - UI_BOX_WIDTH - padding ,
			                          UI_TOP_RESOURCES_Y + UI_MAIN_HALFHEIGHT - padding - UI_ACTIONBUTTON_HEIGHT,
			                          UI_BOX_WIDTH,
			                          UI_ACTIONBUTTON_HEIGHT), 
			                actionButtonLabel)){
				bool hasRequirements = true;

				// check player has enough resources
				foreach(KeyValuePair<Resource,int> entry in levelObject.cost){
					int cost = ((int)(entry.Value * levelObject.levelCostMultiplier * (level +1)));
					int playerResourceAmount = player.currentPlanet.resources[entry.Key];
					hasRequirements = hasRequirements && cost<= playerResourceAmount;
				}

				// check prequisites are all met
				hasRequirements = hasRequirements && levelObject.isMet(player.currentPlanet);

				// check nothing in timer
				hasRequirements = hasRequirements && timeLeft<=0;

				// if everything is ok, deduct costs from player and start building
				if(hasRequirements){
					// deduct costs
					foreach(KeyValuePair<Resource,int> entry in levelObject.cost){
						int cost = ((int)(entry.Value * levelObject.levelCostMultiplier * (level +1)));
						player.currentPlanet.resources[entry.Key] -= cost;
					}
					// add build timer
					if(player.currentPlanet.buildTimers.Count<=levelObject.getTimerSlot()){
						// build Timers is too small. Add to it and fill any intervening time slots with null
						player.currentPlanet.buildTimers.AddRange(new Timer[levelObject.getTimerSlot()+1 - player.currentPlanet.buildTimers.Count]);
					}
					player.currentPlanet.buildTimers[levelObject.getTimerSlot()] = new Timer(levelObject, player.currentPlanet, (int)(levelObject.baseTime + (levelObject.getLevel(player.currentPlanet) * levelObject.baseTime * levelObject.levelTimeMultiplier)));

				}
			}
		} else {
			// nothing selected, so paint a pretty picture
			uiBackground(currentX, currentY, UI_MAIN_WIDTH, UI_MAIN_HALFHEIGHT, label, backgroundTexture, 2);
		}

		
		currentX = UI_MENU_WIDTH;
		currentY = UI_TOP_RESOURCES_Y + UI_MAIN_HALFHEIGHT;
		
		// list the items to select
		uiListObjects(ref currentX, ref currentY, energyBalance, items, padding, rightPadding);
	}
	//----------------------------------------------------//

	/// <summary>
	/// lists a bunch of items to be selected in the main view (e.g. ships in Shipyard view, technologies in Research view)
	/// </summary>
	/// <param name="currentX">X position of top left corner to draw</param>
	/// <param name="currentY">Y position of top left corner to draw</param>
	/// <param name="energyBalance">If set to <c>true</c> show energy balance of items</param>
	/// <param name="items">List of LevelObject (items) to</param>
	/// <param name="padding">Padding. Usually 10</param>
	/// <param name="rightPadding">Right padding for bottom half (item selection). See use in Resources screen for example of it being used to make space for energy management view. Usually 0 though</param>
	/// <typeparam name="T">The type of LevelObject for the list of possible items to select (e.g. Technology, Building, ShipType<)/typeparam>
	private void uiListObjects<T>(ref int currentX, ref int currentY, bool energyBalance, List<T> items, int padding, int rightPadding) where T : LevelObject{

		// background box
		GUI.Box(new Rect(currentX, currentY,UI_MAIN_WIDTH,UI_MAIN_HALFHEIGHT),"");
		currentY += padding;
		currentX += padding;

		// loop through items
		foreach(LevelObject obj in items){
			// check item type. This is a bit of a hack to get only ResourceBuildings and buildings that produce power to appear in
			// Resources view, while other views show whatever list you give them
			if(!energyBalance || obj.GetType()==typeof(ResourceBuilding) || (obj.GetType()==typeof(Building) && ((Building)obj).energy>0)){
				// underlying button that changes selection when pressed
				if (GUI.Button (new Rect (currentX,currentY,UI_BOX_WIDTH,UI_BOX_WIDTH), "")){
					objSelected = obj;
				}				
				// button icon
				int level = obj.getLevel(player.currentPlanet);
				int timeLeft = obj.timeLeft(player.currentPlanet);
				if(timeLeft>0 && !obj.isBuilding(player.currentPlanet)){
					timeLeft = -1;
				}

				uiLayoutButton(ref currentX, ref currentY, obj.icon, level.ToString(), timeLeft, padding, rightPadding);
				
			}
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// draws an icon and label ontop of a button. (Note: does not draw the actual button).
	/// </summary>
	/// <param name="currentX">X position of top left corner to draw</param>
	/// <param name="currentY">Y position of top left corner to draw</param>
	/// <param name="texture">Texture / picture to use for the button</param>
	/// <param name="label">Label to use for the button</param>
	/// <param name="time">Time left till building/researching/etc queue slot is available. Use -1 to indicate unavailable, 0 = available, >0 the time will be displayed on the button. (i.e. this thing is being built and the time is how long left it has till completion) </param>
	/// <param name="padding">Padding.</param>
	/// <param name="rightPadding">Right padding. Used to increment current X position for next button. See uiListObjects for details</param>
	private void uiLayoutButton(ref int currentX, ref int currentY, Texture texture, string label, int time, int padding, int rightPadding){
		// button icon
		GUI.DrawTexture(new Rect(currentX + UI_BOX_IMG_PADDING, currentY + UI_BOX_IMG_PADDING, UI_BOX_WIDTH - (UI_BOX_IMG_PADDING*2),UI_BOX_WIDTH - (UI_BOX_IMG_PADDING*2)),texture);

		// time
		if(time!=0){
			// greyed out due to timer
			GUI.Box(new Rect(currentX, currentY, UI_BOX_WIDTH,UI_BOX_WIDTH),"");
		}
		if(time>0){
			// time till finished
			GUI.Label(new Rect(currentX, currentY + (UI_BOX_WIDTH/2) - (UI_BOX_LABEL_HEIGHT/2), UI_BOX_WIDTH, UI_BOX_LABEL_HEIGHT),
			          string.Format("{0:00}:{1:00}:{2:00}",time/3600,(time/60)%60,time%60), resourceBoxStyle);
		}

		// label (usually current level)
		GUI.Label(new Rect(currentX + UI_BOX_IMG_PADDING, currentY + UI_BOX_WIDTH,UI_BOX_WIDTH,UI_BOX_LABEL_HEIGHT), label);
		currentX += UI_BOX_WIDTH + padding;
		if(currentX + UI_BOX_WIDTH + padding > UI_MENU_WIDTH + UI_MAIN_WIDTH - rightPadding){
			currentX = UI_MENU_WIDTH + padding;
			currentY += UI_BOX_WIDTH + UI_BOX_LABEL_HEIGHT + padding;
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// creates a button that when clicked, hands input control to the specified delegate (uiDataEntry onClick)
	/// </summary>
	/// <param name="currentX">X position of top left corner to draw</param>
	/// <param name="currentY">Y position of top left corner to draw</param>
	/// <param name="width">Width of button</param>
	/// <param name="height">Height of button</param>
	/// <param name="padding">Padding. Usually 10</param>
	/// <param name="onClick">Delegate to fire On click / delegate that will be used to handle input after this button is pressed</param>
	/// <param name="label">label to attach to button</param>
	/// <param name="nonInputLabel">value/label to attach to button when not taking input</param>
	/// <param name="dataEntryView">ID of the view currently taking input</param>
	private void uiKeyboardButton(ref int currentX, ref int currentY, int width, int height, int padding, uiDataEntry onClick, string label, string nonInputLabel, int dataEntryView){
		int tempX = currentX;
				
		GUI.Label(new Rect(currentX, currentY,width,height),label);
		currentY += height;
		string valueLabel = dataEntryBelongsToView==dataEntryView?dataEntered:nonInputLabel;
		if(GUI.Button(new Rect(currentX, currentY, width, height),valueLabel)){
			if(uiTakeKeyboardInput==null){
				uiTakeKeyboardInput = onClick;
				dataEntryBelongsToView = dataEntryView;
				dataEntered = nonInputLabel;
			} else {
				uiTakeKeyboardInput = null;
				dataEntryBelongsToView = 0;
			}
		}
		currentX = tempX;
		currentY += height;
		if(uiTakeKeyboardInput==null || dataEntryBelongsToView!=dataEntryView){
			GUI.Label(new Rect(currentX, currentY, width, height),"(Click to Change)");
		} else {
			GUI.Label(new Rect(currentX , currentY, width, height),"(Enter to Accept)");
		}
		currentY += height + padding;
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region handle input
	//----------------------------------------------------//
	// Methods (usually delegate methods) used for handling
	// input
	//----------------------------------------------------//

	/// <summary>
	/// takes positive integers only from keyboard and places them into dataEntered
	/// </summary>
	public void uiPositiveIntegerInput(){
		if (Input.GetKeyDown(KeyCode.Backspace)){
			if(dataEntered.Length>0){
				dataEntered = dataEntered.Remove(dataEntered.Length-1);
			}
		}
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			uiTakeKeyboardInput = null;
			dataEntryBelongsToView = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){
			dataEntered += "0";
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
			dataEntered += "1";
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
			dataEntered += "2";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){
			dataEntered += "3";
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
			dataEntered += "4";
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)){
			dataEntered += "5";
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)){
			dataEntered += "6";
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)){
			dataEntered += "7";
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)){
			dataEntered += "8";
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)){
			dataEntered += "9";
		}

		int max = 7; // max string length. I.e. can't build >10mil units in one go
		if(dataEntered.Length>max){
			dataEntered = dataEntered.Remove(dataEntered.Length-1);
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// takes positive and negative integers only from keyboard and places them into dataEntered
	/// </summary>
	public void uiIntegerInput(){
		if (Input.GetKeyDown(KeyCode.Backspace)){
			if(dataEntered.Length>0){
				dataEntered = dataEntered.Remove(dataEntered.Length-1);
			}
		}
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			uiTakeKeyboardInput = null;
			dataEntryBelongsToView = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){
			dataEntered += "0";
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
			dataEntered += "1";
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
			dataEntered += "2";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){
			dataEntered += "3";
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
			dataEntered += "4";
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)){
			dataEntered += "5";
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)){
			dataEntered += "6";
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)){
			dataEntered += "7";
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)){
			dataEntered += "8";
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)){
			dataEntered += "9";
		}
		if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)){
			if(!dataEntered.Contains("-")){
				dataEntered = "-" + dataEntered;
			} else {
				dataEntered = dataEntered.Substring(1);
			}
		}
		
		int max = 7; // max string length. I.e. can't build >10mil units in one go
		if(dataEntered.Length>max){
			dataEntered = dataEntered.Remove(dataEntered.Length-1);
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// takes decimal only from keyboard and places them into dataEntered
	/// </summary>
	public void uiDecimalInput(){
		if (Input.GetKeyDown(KeyCode.Backspace)){
			if(dataEntered.Length>0){
				dataEntered = dataEntered.Remove(dataEntered.Length-1);
			}
		}
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			uiTakeKeyboardInput = null;
			dataEntryBelongsToView = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){
			dataEntered += "0";
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
			dataEntered += "1";
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
			dataEntered += "2";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){
			dataEntered += "3";
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
			dataEntered += "4";
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)){
			dataEntered += "5";
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)){
			dataEntered += "6";
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)){
			dataEntered += "7";
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)){
			dataEntered += "8";
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)){
			dataEntered += "9";
		}
		if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.KeypadPeriod)){
			if(!dataEntered.Contains(".")){
				dataEntered += ".";
			}
		}
		if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)){
			if(!dataEntered.Contains("-")){
				dataEntered = "-" + dataEntered;
			} else {
				dataEntered = dataEntered.Substring(1);
			}
		}

		int max = 7; // max string length. I.e. can't build >10mil units in one go
		if(dataEntered.Length>max){
			dataEntered = dataEntered.Remove(dataEntered.Length-1);
		}
	}
	//----------------------------------------------------//

	/// <summary>
	/// takes characters from keyboard and places them into dataEntered
	/// </summary>
	public void uiStringInput(){
		if (Input.GetKeyDown(KeyCode.Backspace)){
			if(dataEntered.Length>0){
				dataEntered = dataEntered.Remove(dataEntered.Length-1);
			}
		} else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			uiTakeKeyboardInput = null;
			dataEntryBelongsToView = 0;
		}
		bool caps = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		string input = "";
		if (Input.GetKey(KeyCode.Space))
		{
			input += "";
		}
		#region data input
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
			dataEntered += "1";
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
			dataEntered += "2";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){
			dataEntered += "3";
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
			dataEntered += "4";
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)){
			dataEntered += "5";
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)){
			dataEntered += "6";
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)){
			dataEntered += "7";
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)){
			dataEntered += "8";
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)){
			dataEntered += "9";
		}
		if (Input.GetKey(KeyCode.A))
		{
			input += "a";
		}
		if (Input.GetKey(KeyCode.B))
		{
			input += "b";
		}
		if (Input.GetKey(KeyCode.C))
		{
			input += "c";
		}
		if (Input.GetKey(KeyCode.D))
		{
			input += "d";
		}
		if (Input.GetKey(KeyCode.E))
		{
			input += "e";
		}
		if (Input.GetKey(KeyCode.F))
		{
			input += "f";
		}
		if (Input.GetKey(KeyCode.G))
		{
			input += "g";
		}
		if (Input.GetKey(KeyCode.H))
		{
			input += "h";
		}
		if (Input.GetKey(KeyCode.I))
		{
			input += "i";
		}
		if (Input.GetKey(KeyCode.J))
		{
			input += "j";
		}
		if (Input.GetKey(KeyCode.K))
		{
			input += "k";
		}
		if (Input.GetKey(KeyCode.L))
		{
			input += "l";
		}
		if (Input.GetKey(KeyCode.M))
		{
			input += "m";
		}
		if (Input.GetKey(KeyCode.N))
		{
			input += "n";
		}
		if (Input.GetKey(KeyCode.O))
		{
			input += "o";
		}
		if (Input.GetKey(KeyCode.P))
		{
			input += "p";
		}
		if (Input.GetKey(KeyCode.Q))
		{
			input += "q";
		}
		if (Input.GetKey(KeyCode.R))
		{
			input += "r";
		}
		if (Input.GetKey(KeyCode.S))
		{
			input += "s";
		}
		if (Input.GetKey(KeyCode.T))
		{
			input += "t";
		}
		if (Input.GetKey(KeyCode.U))
		{
			input += "u";
		}
		if (Input.GetKey(KeyCode.V))
		{
			input += "v";
		}
		if (Input.GetKey(KeyCode.W))
		{
			input += "w";
		}
		if (Input.GetKey(KeyCode.X))
		{
			input += "x";
		}
		if (Input.GetKey(KeyCode.Y))
		{
			input += "y";
		}
		if (Input.GetKey(KeyCode.Z))
		{
			input += "z";
		}

		if(caps){
			input = input.ToUpper();
		}
		dataEntered += input;
		#endregion	

		int max = 64; // max string length. I.e. can't build >10mil units in one go
		if(dataEntered.Length>max){
			dataEntered = dataEntered.Remove(dataEntered.Length-1);
		}
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region logic utils
	//----------------------------------------------------//
	// Helper utils for game logic
	//----------------------------------------------------//

	/// <summary>
	/// energy after all production and consumption has been taken into account
	/// </summary>
	/// <returns>The energy balance.</returns>
	/// <param name="planet">Planet.</param>
	private static int getEnergyBalance(Planet planet){
		int totalEnergy = 0;
		foreach(KeyValuePair<Building,int> entry in planet.buildingsLevels){
			totalEnergy += (int)(entry.Key.energy * entry.Key.levelEnergyMultiplier * entry.Value * planet.buildingsEnergy[entry.Key]);
		}
		return totalEnergy;
	}
	//----------------------------------------------------//

	/// <summary>
	/// energy produced, not including energy consumed
	/// </summary>
	/// <returns>The energy produces.</returns>
	/// <param name="planet">Planet.</param>
	private static int getEnergyProduces(Planet planet){
		int totalEnergy = 0;
		foreach(KeyValuePair<Building,int> entry in planet.buildingsLevels){
			int energy = (int)(entry.Key.energy * entry.Key.levelEnergyMultiplier * entry.Value * planet.buildingsEnergy[entry.Key]);
			if(energy>0){
				totalEnergy+=energy;
			}
		}
		return totalEnergy;
	}
	//----------------------------------------------------//

	/// <summary>
	/// The base (resource) output of the building, given it's level on the specified planet. Usually a ResourceBuilding
	/// </summary>
	/// <returns>The base output.</returns>
	/// <param name="building">Building.</param>
	/// <param name="planet">Planet.</param>
	private static int getBaseOutput(Building building, Planet planet){
		if(building.GetType()!=typeof(ResourceBuilding)){
			return 0;
		}
		int level = planet.buildingsLevels[building];
		return (int)(((ResourceBuilding)building).baseAmount * ((ResourceBuilding)building).levelResourceMultiplier * level);
	}
	//----------------------------------------------------//

	/// <summary>
	/// Energy needed by the specified building given its level and (user set in energy management) energy percentage on the specified planet
	/// </summary>
	/// <returns>The energy needed.</returns>
	/// <param name="building">Building.</param>
	/// <param name="planet">Planet.</param>
	private static int getEnergyNeeded(Building building, Planet planet){
		int level = planet.buildingsLevels[building];
		return (int)(level * building.levelEnergyMultiplier * building.energy);
	}
	//----------------------------------------------------//

	/// <summary>
	/// The actual energy used by the building, given its level and energy percentage on the specified planet, and also taking into account the energy balance on the planet (if there isn't enough energy overall on the planet, this will be accounted for here)
	/// </summary>
	/// <returns>The actual energy.</returns>
	/// <param name="building">Building.</param>
	/// <param name="planet">Planet.</param>
	private static float getActualEnergy(Building building, Planet planet){
		// get energy useage as a percent
		float energy = planet.buildingsEnergy[building];
		int energyBalance = getEnergyBalance(planet);
		if(energyBalance<0){
			// energy shortfall across planet. adjust our own energy accordingly
			
			int energyProduced = getEnergyProduces(planet);
			if(energyProduced<=0){
				energy = 0.0f;
			} else {
				// energy shortfall as a percent of total energy
				float energyPercent = 1.0f + ((float)energyBalance / (float)energyProduced);
				if(energyPercent<=0.0f){
					// shortfall is more than total produced! No production at all!
					energy = 0.0f;
				} else {
					// depending on the shortfall, adjust our energy accordingly
					energy = Mathf.Min(energy, energyPercent);
				}
			}
		}
		return energy;
	}
	//----------------------------------------------------//

	/// <summary>
	/// Gets the actual output of the building. NOTE: does not accurately model how OGame handles this
	/// </summary>
	/// <returns>The actual output.</returns>
	/// <param name="building">Building.</param>
	/// <param name="planet">Planet.</param>
	private static int getActualOutput(Building building, Planet planet){
		return (int)(getBaseOutput(building, planet) * getActualEnergy(building, planet));
	}
	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//

	//----------------------------------------------------//
	#region classes
	//----------------------------------------------------//
	// Classes for various different objects in the game.
	// Things like planets, player, buildings, resources etc
	//----------------------------------------------------//

	/// planet. This is where most of the instanced data in the game is stored
	public class Planet{
		public string name;
		public int diameter;
		public int maxTemperature;
		public int minTemperature;
		public SolarSystem system;
		public Player owner;

		/// resource stockpiles at the planet. 
		/// These are what are shown at the top of the screen in the Top Resources ui view
		public Dictionary<Resource, int> resources = new Dictionary<Resource, int>(); 

		/// levels of the buildings on this planet
		public Dictionary<Building, int> buildingsLevels = new Dictionary<Building, int>();

		/// energy levels of buildings on this planet (as set in Energy Management in Resources ui)
		public Dictionary<Building, float> buildingsEnergy = new Dictionary<Building, float>();

		/// technology levels of planet. Note: In actual OGame these are global, not per-planet
		public Dictionary<Technology, int> technologyLevels = new Dictionary<Technology, int>();

		/// hangar / what ships are at this planet. This is used as the substitute for "levels" for ships
		public Dictionary<ShipType, int> hangar = new Dictionary<ShipType, int>();

		/// each planet has a number of build spots. The "Timer" object at each "slot" (position in the array)
		/// represents something counting down to being built.
		/// To make two LevelObjects unable to be built at the same time, make them return the same slot number from LevelObject.getTimerSlot()
		public List<Timer> buildTimers = new List<Timer>();

		public Planet(SolarSystem _system){
			name = "";
			diameter = Random.Range(400,12000) * 2;
			minTemperature = Random.Range(-100,80);
			maxTemperature = minTemperature + Random.Range(25,100);
			system = _system;

			// fill dictionaries / arrays
			foreach(Resource res in logic.baseResources){
				resources.Add(res,0);
			}

			foreach(Building building in baseAllBuildings){
				buildingsLevels.Add(building,0);
				buildingsEnergy.Add(building, 1.0f);

			}

			foreach(Technology tech in baseTechnologies){
				technologyLevels.Add(tech,0);
			}

			foreach(ShipType type in baseShipTypes){
				hangar.Add(type,0);
			}

		}

		/// <summary>
		/// this is where all the update logic happens
		/// </summary>
		public void Update(){

			// loop through each building and gather any resources it produces
			foreach(KeyValuePair<Building, int> entry in buildingsLevels){
				Building building = entry.Key;
				if(building.GetType()==typeof(ResourceBuilding)){
					ResourceBuilding resBuilding = (ResourceBuilding)building;

					// add resources depending on how much actually output
					resources[resBuilding.resource] += (int)(getActualOutput(building, this) * (float)updateInterval / secondsInMinute);
				}
			}

			// loop through build timers
			for(int i = 0;i<buildTimers.Count;i++){
				Timer timer = buildTimers[i];
				if(timer!=null){
					int timeLeft = timer.timeLeft;
					timeLeft -= (int)(60.0f / secondsInMinute * updateInterval);
					if(timeLeft<0){
						buildTimers[i] = null;
						timer.obj.setLevel(this, timer.obj.getLevel(this) + 1);
						i--;
					} else {
						timer.timeLeft = timeLeft;
					}
				}
			}

		}
	}

	/// solar system. A collection of planets
	public class SolarSystem{

		public List<Planet> planets = new List<Planet>();

	}
	//----------------------------------------------------//
	//----------------------------------------------------//

	/// player. A collection of planets
	public class Player{
		public string name;
		public List<Planet> planets = new List<Planet>();
		public Planet currentPlanet; // the currently selected planet. This is the planet to show resources for, construct buildings on etc
	}

	//----------------------------------------------------//
	//----------------------------------------------------//

	/// resources. Minerals, crystals and stuff the player collects to later use for building/researching etc
	public class Resource{
		public string name;
		public Texture texture;
	}

	//----------------------------------------------------//
	//----------------------------------------------------//

	/// Building
	public class Building : LevelObject {
		public static int timerSlot = 0;

		/// base energy amount produced at level 0. +ve for generates, -ve for consumes, 0 = energy neutral
		public int energy; 

		/// the multiplier for the amount of energy the building consumes per level
		public float levelEnergyMultiplier; 

		public override int getLevel(Planet planet){
			return planet.buildingsLevels[this];
		}
		public override void setLevel(Planet planet, int level){
			planet.buildingsLevels[this] = level;
		}

		public override bool isMet(Planet planet){
			return timeLeft(planet)<=0 && base.isMet(planet);
		}

		public override int getTimerSlot (){
			return timerSlot;
		}
	}

	/// Building that produces/consumes resources over time
	public class ResourceBuilding : Building{
		public Resource resource;

		/// base resource amount produced at level 0
		public int baseAmount; 

		/// multiplier for the amount of resources the building produces/consumes per level	
		public float levelResourceMultiplier;	
	}

	/// facility type building
	public class FacilitiesBuilding : Building{
	}

	//----------------------------------------------------//
	//----------------------------------------------------//

	/// technologies the player can research
	public class Technology : LevelObject {
		public static int timerSlot = 1;
		public override int getLevel(Planet planet){
			return planet.technologyLevels[this];
		}
		public override void setLevel(Planet planet, int level){
			planet.technologyLevels[this] = level;
		}

		public override bool isMet(Planet planet){
			return timeLeft(planet)<=0 && base.isMet(planet);
		}

		public override int getTimerSlot (){
			return timerSlot;
		}
	}

	//----------------------------------------------------//
	//----------------------------------------------------//

	/// ships the player can produce
	public class ShipType : LevelObject{
		public static int timerSlot = 2;

		public override int getLevel(Planet planet){
			return planet.hangar[this];
		}
		public override void setLevel(Planet planet, int level){
			planet.hangar[this] = level;
		}

		public override bool isMet(Planet planet){
			return timeLeft(planet)<=0 && base.isMet(planet);
		}

		public override int getTimerSlot (){
			return timerSlot;
		}
	}

	//----------------------------------------------------//
	//----------------------------------------------------//

	//----------------------------------------------------//
	// The base class / important one
	//----------------------------------------------------//
	/// higher level object / item in game
	public abstract class LevelObject{
		public string name;
		public Texture icon;

		/// base time to go to next level
		public int baseTime; 

		/// multiplier by which time to next level goes up per level
		public float levelTimeMultiplier; 

		/// multiplier by which cost goes up on base amount per level
		public float levelCostMultiplier; 

		/// base cost to build / level up.
		public Dictionary<Resource, int> cost = new Dictionary<Resource, int>(); 

		public abstract int getLevel(Planet planet);
		public abstract void setLevel(Planet planet, int level);
				
		/// prerequisites. Things needed before this item can be levelled up.
		public Dictionary<LevelObject, int> prequisites = new Dictionary<LevelObject, int>(); 

		/// <summary>
		/// are all prerequisites met?
		/// </summary>
		/// <returns><c>true</c>, if prerequisites, <c>false</c> otherwise.</returns>
		/// <param name="planet">Planet.</param>
		public virtual bool isMet(Planet planet){
			foreach(KeyValuePair<LevelObject, int> entry in prequisites){
				if(entry.Key.getLevel(planet)<entry.Value){
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// The building "slot" on the planet. To prevent two objects building at the same time, give them the same "slot" number
		/// </summary>
		/// <returns>The timer slot.</returns>
		public abstract int getTimerSlot();

		/// <summary>
		// returns true if this object is building on the specificed planet
		/// </summary>
		/// <returns><c>true</c>, if this object is building on the specificed planet, <c>false</c> otherwise.</returns>
		/// <param name="planet">Planet.</param>
		public bool isBuilding(Planet planet){
			int slot = getTimerSlot();
			return planet.buildTimers.Count-1<=slot && planet.buildTimers[slot]!=null && planet.buildTimers[slot].obj == this;
		}

		/// <summary>
		/// time left till this object's slot is free on the specified planet
		/// </summary>
		/// <returns>The time left.</returns>
		/// <param name="planet">Planet.</param>
		public int timeLeft(Planet planet){
			int slot = getTimerSlot();
			if(planet.buildTimers.Count<=slot || planet.buildTimers[slot]==null){
				return 0;
			}
			return planet.buildTimers[slot].timeLeft;
		}

	}

	//----------------------------------------------------//

	/// Used for representing a levelobject in the process of being upped one level
	public class Timer{
		public LevelObject obj;
		public Planet planet;
		public int timeLeft;
		public Timer(LevelObject pObj, Planet pPlanet, int pTimeLeft){
			obj = pObj;
			planet = pPlanet;
			timeLeft = pTimeLeft;
		}
	}

	//----------------------------------------------------//
	#endregion
	//----------------------------------------------------//
}
