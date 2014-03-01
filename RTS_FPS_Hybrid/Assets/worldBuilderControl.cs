using UnityEngine;
using System.Collections;

public class worldBuilderControl : MonoBehaviour 
{
	
	public GameObject pillarPrefab;
	public Vector2 initialCenterPosition = new Vector2(0.0f, 0.0f);
	public float maxHeight;
	public float minHeight;
	public int percentMinHeight;
	public int percentMaxHeight;
	public int xNumberOfCells;
	public int yNumberOfCells;
	public float pillarDelay;
	public float mapEdgeHeight;
	public int radiusMin;
	public int radiusMax;
	
	private GameObject pillarGameObject;
	private float timeForNextPillar = 0.0f;
	private	float xOffset1 = 0.0f;
	private float xOffset2 = 0.0f;
	private float yOffset1 = 0.0f;
	// Data matrix for cell radii and heights -> pillarCellData[centerRadius,centerHeight]
	private Vector2[,] pillarCellData;
	
	
	
	
	
	// Use this for initialization
	void Start() 
	{
		pillarCellData = new Vector2[xNumberOfCells,yNumberOfCells]; 
		
		BuildGrid();
		
		FillCells();
		
	}
	
	
	
	
	
	void BuildGrid()
	{
		xOffset1 = (1.5f*((2.0f*(float)radiusMax)-1.0f));
		xOffset2 = (1.5f*((float)radiusMax-1.0f));
		yOffset1 = ((((float)radiusMax/2.0f)+((float)radiusMax-1.0f))*2.0f)+1.0f;
		
		// Loop to create a (xNumberOfCells X yNumberOfCells) grid
		for(float yIterator = 0.0f; yIterator<yNumberOfCells; yIterator++)
		{
			for(float xIterator = 0.0f; xIterator<xNumberOfCells; xIterator++)
			{
			
	   			pillarCellData[(int)xIterator,(int)yIterator] = BuildCell(new Vector2((initialCenterPosition.x+(xOffset1*xIterator)+(xOffset2*(yIterator%2.0f))-(1.5f*Mathf.Floor(yIterator/2.0f))),(initialCenterPosition.y+(yOffset1*yIterator)+(1.0f*xIterator)-(1.0f*Mathf.Floor(yIterator/2.0f)))));
			}
		}
		
	}
	
	
	Vector2 BuildCell(Vector2 centerPosition)
	{
		// Create initial center with random radius and height
		int centerRadius = Random.Range(radiusMin,radiusMax);
		int centerHeightSelect = Random.Range(1,100);
		float centerHeight;
		
		// Establish center height
		if(centerHeightSelect < percentMinHeight)
		{
		   centerHeight = minHeight;
		}
		else if (centerHeightSelect < (percentMinHeight + percentMaxHeight))
		{
		   centerHeight = maxHeight;	
		}
		else 
		{
		   centerHeight = Random.Range(minHeight,maxHeight);	
		}
		
		Vector2 returnValue = new Vector2((float)centerRadius,centerHeight);
		
		// Create pillars for center
		Quaternion pillarRotation = Quaternion.identity;
		pillarRotation.eulerAngles = new Vector3(90.0f,90.0f,0.0f);
		float xOffset;
		float yOffset;
		
		// Place center columns
		pillarGameObject = Instantiate(pillarPrefab, new Vector3(centerPosition.x,20.0f,centerPosition.y), pillarRotation) as GameObject;
		pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,centerHeight);

		for(int currentRadius=2; currentRadius<=centerRadius; currentRadius++)
		{
			for(xOffset=-(1.5f*(currentRadius-1.0f)); xOffset<=(1.5f*(currentRadius-1.0f)); xOffset+=1.5f)
			{
				yOffset = (2.0f*(currentRadius-1.0f))-Mathf.Abs(xOffset/1.5f);
		        pillarGameObject = Instantiate(pillarPrefab, new Vector3(centerPosition.x+xOffset,20.0f,centerPosition.y+yOffset), pillarRotation) as GameObject;
		        pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,centerHeight);
			    pillarGameObject = Instantiate(pillarPrefab, new Vector3(centerPosition.x+xOffset,20.0f,centerPosition.y-yOffset), pillarRotation) as GameObject;
		        pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,centerHeight);
				//yield WaitForSeconds(0.2f);
			}
			if(currentRadius>2)
			{
			   for(yOffset=-(currentRadius-3.0f); yOffset<=(currentRadius-3.0f); yOffset+=2.0f)
			   {
			      pillarGameObject = Instantiate(pillarPrefab, new Vector3(centerPosition.x+(1.5f*(currentRadius-1.0f)),20.0f,centerPosition.y+yOffset), pillarRotation) as GameObject;
		          pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,centerHeight);
				  pillarGameObject = Instantiate(pillarPrefab, new Vector3(centerPosition.x-(1.5f*(currentRadius-1.0f)),20.0f,centerPosition.y+yOffset), pillarRotation) as GameObject;
		          pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,centerHeight);
				  //yield WaitForSeconds(0.2f);
			   }
			}
		}
				
		return returnValue;
	}
	
	
	
	
	void FillCells()
	{
		// Place remaining columns
		// Create a data vector for each diagonal of the current cell
		// diag# = (numIncrements,incrementHeight)
		Vector2 diag0 = new Vector2();
		Vector2 diag1 = new Vector2();
		Vector2 diag2 = new Vector2();
		Vector2 diag3 = new Vector2();
		Vector2 diag4 = new Vector2();
		Vector2 diag5 = new Vector2();
		Vector2 fill = new Vector2();
		Vector2 currentCellPosition = new Vector2();
		Vector2 currentCell = new Vector2();
		Vector2 cell1 = new Vector2();
		Vector2 cell2 = new Vector2();
		Vector2 diag0Position = new Vector2(0.0f,0.0f);
		Vector2 diag1Position = new Vector2(0.0f,0.0f);
		Vector2 diag2Position = new Vector2(0.0f,0.0f);
		Vector2 diag3Position = new Vector2(0.0f,0.0f);
		Vector2 diag4Position = new Vector2(0.0f,0.0f);
		Vector2 diag5Position = new Vector2(0.0f,0.0f);
		int isCurrentCellOdd;
		float heightDifference;
		float numberOfIncrements;
		float incrementHeight;
		xOffset1 = (1.5f*((2.0f*(float)radiusMax)-1.0f));
		xOffset2 = (1.5f*((float)radiusMax-1.0f));
		yOffset1 = ((((float)radiusMax/2.0f)+((float)radiusMax-1.0f))*2.0f)+1.0f;
		
		
		for(float yIterator = 0.0f; yIterator<yNumberOfCells; yIterator++)
		{
			for(float xIterator = 0.0f; xIterator<xNumberOfCells; xIterator++)
			{
				currentCell = pillarCellData[(int)xIterator,(int)yIterator];
	   			currentCellPosition = new Vector2((initialCenterPosition.x+(xOffset1*xIterator)+(xOffset2*(yIterator%2.0f))-(1.5f*Mathf.Floor(yIterator/2.0f))),(initialCenterPosition.y+(yOffset1*yIterator)+(1.0f*xIterator)-(1.0f*Mathf.Floor(yIterator/2.0f))));
				
				// Build diag0
				// Find out if the current cell is in an odd or even Y-tier
				isCurrentCellOdd = (int)(yIterator%2.0f);
				
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag0
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)xIterator,(int)(yIterator+1)];
						cell2 = pillarCellData[(int)(xIterator+1),(int)(yIterator+1)];
						
					}
				}
				else
				{
					// For even cell, diag0
					if((xIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator-1),(int)(yIterator+1)];
						cell2 = pillarCellData[(int)xIterator,(int)(yIterator+1)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag0 = new Vector2(numberOfIncrements,incrementHeight);
				
				
				// Build diag1
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag1
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator+1),(int)(yIterator+1)];
						cell2 = pillarCellData[(int)(xIterator+1),(int)(yIterator)];
						
					}
				}
				else
				{
					// For even cell, diag1
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)xIterator,(int)(yIterator+1)];
						cell2 = pillarCellData[(int)(xIterator+1),(int)(yIterator)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag1 = new Vector2(numberOfIncrements,incrementHeight);
			
				
				
				
				// Build diag2
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag2
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator+1),(int)(yIterator)];
						cell2 = pillarCellData[(int)(xIterator+1),(int)(yIterator-1)];
						
					}
				}
				else
				{
					// For even cell, diag2
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)xIterator,(int)(yIterator-1)];
						cell2 = pillarCellData[(int)(xIterator+1),(int)(yIterator)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag2 = new Vector2(numberOfIncrements,incrementHeight);
				
				
				
				// Build diag3
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag3
					if((xIterator+1)>(xNumberOfCells-1))
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator+1),(int)(yIterator-1)];
						cell2 = pillarCellData[(int)(xIterator),(int)(yIterator-1)];
						
					}
				}
				else
				{
					// For even cell, diag3
					if((xIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator-1),(int)(yIterator-1)];
						cell2 = pillarCellData[(int)(xIterator),(int)(yIterator-1)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag3 = new Vector2(numberOfIncrements,incrementHeight);
				
				
				
				
				// Build diag4
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag4
					if((xIterator-1)<0)
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator),(int)(yIterator-1)];
						cell2 = pillarCellData[(int)(xIterator-1),(int)(yIterator)];
						
					}
				}
				else
				{
					// For even cell, diag4
					if((xIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator-1)<0)
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator-1),(int)(yIterator)];
						cell2 = pillarCellData[(int)(xIterator-1),(int)(yIterator-1)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag4 = new Vector2(numberOfIncrements,incrementHeight);
				
				
				
				
				// Build diag5
				// Load the radius and height values of the required cells
				if(isCurrentCellOdd>0)
				{
					// For odd cell, diag5
					if((xIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator-1),(int)(yIterator)];
						cell2 = pillarCellData[(int)(xIterator),(int)(yIterator+1)];
						
					}
				}
				else
				{
					// For even cell, diag5
					if((xIterator-1)<0)
					{
						cell1 = new Vector2(radiusMax, mapEdgeHeight);
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else if((yIterator+1)>(yNumberOfCells-1))
					{
						cell2 = new Vector2(radiusMax, mapEdgeHeight);
					}
					else
					{
						cell1 = pillarCellData[(int)(xIterator-1),(int)(yIterator)];
						cell2 = pillarCellData[(int)(xIterator-1),(int)(yIterator+1)];
					}
				}
				
				// Calculate the desired height difference between the center and the edge of the current cell
				// (Difference between the current cell and the average height of the other two cells)
				heightDifference = ((cell1.y+cell2.y)/2) - currentCell.y;
				
				// Calculate the number of incremental columns available
				numberOfIncrements = ((float)radiusMax-currentCell.x)+((float)radiusMax-Mathf.Max(cell1.x,cell2.x));
				
				// Calculate the required incremental height between columns
				if (numberOfIncrements==0)
				{
					incrementHeight = 0;
				}
				else
				{
					incrementHeight = heightDifference/numberOfIncrements;
				}
				
				diag5 = new Vector2(numberOfIncrements,incrementHeight);
				
				
				
				Quaternion pillarRotation = Quaternion.identity;
				pillarRotation.eulerAngles = new Vector3(90.0f,90.0f,0.0f);
				float diag0Height = currentCell.y;
				float diag1Height = currentCell.y;
				float diag2Height = currentCell.y;
				float diag3Height = currentCell.y;
				float diag4Height = currentCell.y;
				float diag5Height = currentCell.y;
				Vector2 fillPosition = new Vector2(0.0f,0.0f);
				
				// Loop and build each diagonal
				for(int ii=(int)currentCell.x; ii<radiusMax; ii++)
				{
					// Place diag0 column
					diag0Position.x = currentCellPosition.x;
					diag0Position.y = currentCellPosition.y + (2*(ii));
					diag0Height += diag0.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag0Position.x,20.0f,diag0Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag0Height);
						
					// Place diag1 column
					diag1Position.x = currentCellPosition.x + (1.5f*(ii));
					diag1Position.y = currentCellPosition.y + ii;
					diag1Height += diag1.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag1Position.x,20.0f,diag1Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag1Height);
					
					// Calculate fill 1-0 vector
					fill.x = (ii-1);
					fill.y = (diag0Height - diag1Height)/fill.x;
					float fillHeight = diag1Height;
					
					// Place filler columns 1-0
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag0Position.x + (1.5f*(jj));
						fillPosition.y = diag0Position.y - (jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}
					
					// Place diag2 column
					diag2Position.x = currentCellPosition.x + (1.5f*(ii));
					diag2Position.y = currentCellPosition.y - ii;
					diag2Height += diag2.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag2Position.x,20.0f,diag2Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag2Height);
					
					// Calculate fill 2-1 vector
					fill.x = (ii-1);
					fill.y = (diag1Height - diag2Height)/fill.x;
					fillHeight = diag2Height;
					
					// Place filler columns 2-1
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag1Position.x;
						fillPosition.y = diag1Position.y - (2*jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}
					
					// Place diag3 column
					diag3Position.x = currentCellPosition.x;
					diag3Position.y = currentCellPosition.y - (2*ii);
					diag3Height += diag3.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag3Position.x,20.0f,diag3Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag3Height);
					
					// Calculate fill 3-2 vector
					fill.x = (ii-1);
					fill.y = (diag2Height - diag3Height)/fill.x;
					fillHeight = diag3Height;
					
					// Place filler columns 3-2
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag2Position.x - (1.5f*jj);
						fillPosition.y = diag2Position.y - (jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}
					
					// Place diag4 column
					diag4Position.x = currentCellPosition.x - (1.5f*(ii));
					diag4Position.y = currentCellPosition.y - ii;
					diag4Height += diag4.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag4Position.x,20.0f,diag4Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag4Height);
					
					// Calculate fill 4-3 vector
					fill.x = (ii-1);
					fill.y = (diag3Height - diag4Height)/fill.x;
					fillHeight = diag4Height;
					
					// Place filler columns 4-3
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag3Position.x - (1.5f*jj);
						fillPosition.y = diag3Position.y + (jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}
					
					// Place diag5 column
					diag5Position.x = currentCellPosition.x - (1.5f*(ii));
					diag5Position.y = currentCellPosition.y + ii;
					diag5Height += diag5.y;
					pillarGameObject = Instantiate(pillarPrefab, new Vector3(diag5Position.x,20.0f,diag5Position.y), pillarRotation) as GameObject;
					pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,diag5Height);
					
					// Calculate fill 5-4 vector
					fill.x = (ii-1);
					fill.y = (diag4Height - diag5Height)/fill.x;
					fillHeight = diag5Height;
					
					// Place filler columns 5-4
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag4Position.x;
						fillPosition.y = diag4Position.y + (2*jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}
					
					// Calculate fill 5-0 vector
					fill.x = (ii-1);
					fill.y = (diag0Height - diag5Height)/fill.x;
					fillHeight = diag5Height;
					
					// Place filler columns 1-0
					for(int jj=(int)fill.x; jj>0; jj--)
					{
						fillPosition.x = diag0Position.x - (1.5f*(jj));
						fillPosition.y = diag0Position.y - (jj);
						fillHeight += fill.y;
						
						pillarGameObject = Instantiate(pillarPrefab, new Vector3(fillPosition.x,20.0f,fillPosition.y), pillarRotation) as GameObject;
						pillarGameObject.transform.localScale = new Vector3(1.15f,1.0f,fillHeight);
					}					
				}
			}
		}		
	}	
}
