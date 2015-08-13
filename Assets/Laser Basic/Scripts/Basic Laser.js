//Laser Controller
//This code can be used for private or commercial projects but cannot be sold or redistributed without written permission.
//Copyright Nik W. Kraus / Dark Cube Entertainment LLC. 
    #pragma strict
    
    var StartPoint : Transform;
	
	var LaserDirection = "X";
		
	
	var Use2D = false;	
	var LaserMask : LayerMask;
	
	var LaserOn = true;    
    
	var UseUVPan = true;
	
	var EndFlareOffset = 0.0;
	
	var SourceFlare : LensFlare;
	var EndFlare : LensFlare;

	var AddSourceLight = true;
	var AddEndLight = true;

    var LaserColor : Color = Color(1,1,1,.5);
        
    var StartWidth = 0.1;
    var EndWidth = 0.1;
    var LaserDist = 20.0;
	var TexScrollX = -0.1;
	var TexScrollY = 0.1;
	var UVTexScale = Vector2(4,.4);
    
    private var SectionDetail : float = 2;       
    private var lineRenderer : LineRenderer;
    private var ray = Ray(Vector3(0,0,0), Vector3(0,1,0));	
	private var EndPos : Vector3;
	private var hit: RaycastHit;
	private var	hit2D : RaycastHit2D;
	private var SourceLight : GameObject;
	private var EndLight : GameObject;
	private var ViewAngle : float;
	private var LaserDir : Vector3;
	
	
    @script RequireComponent(LineRenderer)
    
    function Start() {
         lineRenderer = GetComponent(LineRenderer);
         if(lineRenderer.material == "none")
         lineRenderer.GetComponent.<Renderer>().material = new Material (Shader.Find("LaserAdditive"));
         
         lineRenderer.castShadows = false;
         lineRenderer.receiveShadows = false;
         
         lineRenderer.SetVertexCount(SectionDetail);
         lineRenderer = GetComponent(LineRenderer);
         
         // Make a lights
        if(AddSourceLight){
		StartPoint.gameObject.AddComponent(Light);
		StartPoint.GetComponent.<Light>().intensity = 1.5;
		StartPoint.GetComponent.<Light>().range = .5;
		}
		
		if(AddEndLight){
			if(EndFlare){
			EndFlare.gameObject.AddComponent(Light);
			EndFlare.GetComponent.<Light>().intensity = 1.5;
			EndFlare.GetComponent.<Light>().range = .5;		
			}
			else{Debug.Log("To use End Light, please assign an End Flare");}
		}
		
		
		if(LaserDirection=="x" || LaserDirection=="y" || LaserDirection=="z" || LaserDirection=="X" || LaserDirection=="Y" || LaserDirection=="Z"){     
        }
        else{
        	Debug.Log("Laser Direction can only be X, Y or Z");}
        
                             
     }//end start
         
    
        /////////////////////////////////////
    function Update() {
    	if(LaserDirection=="x" || LaserDirection=="X"){		
        	LaserDirection="X";
        	LaserDir = StartPoint.right;
        }
        else if(LaserDirection=="y" || LaserDirection=="Y"){
        	LaserDirection="Y";
        	LaserDir = StartPoint.up;
        }
        else if(LaserDirection=="z" || LaserDirection=="Z"){
        	LaserDirection="Z";
        	LaserDir = StartPoint.forward;
        }
        else{
        LaserDir = StartPoint.forward;
        }
    
    	var CamDistSource = Vector3.Distance(StartPoint.position, Camera.main.transform.position);
    	var CamDistEnd = Vector3.Distance(EndPos, Camera.main.transform.position);		
		ViewAngle = Vector3.Angle(LaserDir, Camera.main.transform.forward);		 
    	    	  	    	
      if(LaserOn){
      	lineRenderer.enabled = true;               
        lineRenderer.SetWidth(StartWidth,EndWidth);
        lineRenderer.material.color = LaserColor;
        
        //Flare Control
        if(SourceFlare){
        SourceFlare.color = LaserColor;
        SourceFlare.transform.position = StartPoint.position;
        
        if(ViewAngle > 155 && CamDistSource < 20 && CamDistSource > 0){
        SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,20.0,.001);	
        }
        else{
        SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,0.1,.05);
         }        
        }
        
        if(EndFlare){
        EndFlare.color = LaserColor;
        
        if(CamDistEnd > 20){
        EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,0.0,.1);
        }
        else{
        EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,5.0,.1);
         }
        }// end flare        
        
        //Light Control
        if(AddSourceLight)
        StartPoint.GetComponent.<Light>().color = LaserColor;

        if(AddEndLight){
         if(EndFlare){
         EndFlare.GetComponent.<Light>().color = LaserColor;
         }
        }       
        
        
        
                        
        /////////////////////Ray Hit
       if(Use2D){
        hit2D = Physics2D.Raycast(StartPoint.position, LaserDir, LaserDist, LaserMask);        

        var ray2 = new Ray2D(StartPoint.position, LaserDir);
        var dist2D = Vector3.Distance(StartPoint.position, hit2D.point);
        if (hit2D){		        
		       EndPos = hit2D.point;	 		    
			    
			   if(EndFlare){
			   EndFlare.enabled = true;
			    
			   if(AddEndLight){
			   if(EndFlare){
			   EndFlare.GetComponent.<Light>().enabled = true;		    
		     }
		    }
		      		    
		    if(EndFlareOffset > 0)
		    EndFlare.transform.position = hit2D.point + hit2D.normal * EndFlareOffset;
		    else
		    EndFlare.transform.position = EndPos;
		    }		    
          }
        else{
	        if(EndFlare)
	        EndFlare.enabled = false;	        
	        
	        if(AddEndLight){
		     if(EndFlare){
		     EndFlare.GetComponent.<Light>().enabled = false;
		     }
		    }
		    
           EndPos = ray2.GetPoint(LaserDist);	        	        	        	        
         }
       	}
       	///Else 3D Ray
       else{
       	ray = new Ray(StartPoint.position, LaserDir);			
        if (Physics.Raycast(ray, hit, LaserDist, LaserMask)){	        
	        EndPos = hit.point;	 		    
		    
		    if(EndFlare){
		    EndFlare.enabled = true;
		    
		    if(AddEndLight){
		     if(EndFlare){
		     EndFlare.GetComponent.<Light>().enabled = true;		    
		     }
		    }
		      		    
		    if(EndFlareOffset > 0)
		    EndFlare.transform.position = hit.point + hit.normal * EndFlareOffset;
		    else
		    EndFlare.transform.position = EndPos;
		    }		    
          }
        else{
	        if(EndFlare)
	        EndFlare.enabled = false;	        
	        
	        if(AddEndLight){
		     if(EndFlare){
		     EndFlare.GetComponent.<Light>().enabled = false;
		     }
		    }
		    
           EndPos = ray.GetPoint(LaserDist);	        	        	        	        
         }
        }//end Ray       
        
        
        //Debug.DrawLine (StartPoint.position, EndPos, Color.red);
        
          //Find Distance
	      var dist = Vector3.Distance(StartPoint.position, EndPos);
	      
	      //Line Render Positions
	      lineRenderer.SetPosition(0,StartPoint.position);
	      lineRenderer.SetPosition(1,EndPos);
	                  
    //Texture Scroller
    if(UseUVPan){
    //lineRenderer.material.SetTextureScale("_Mask", Vector2(dist/4, .1));
    lineRenderer.material.SetTextureScale("_Mask", Vector2(dist/UVTexScale.x, UVTexScale.y));
    lineRenderer.material.SetTextureOffset ("_Mask", Vector2(TexScrollX*Time.time, TexScrollY*Time.time));
    }
    
   }
   else{
    lineRenderer.enabled = false;
    
    if(SourceFlare)
    SourceFlare.enabled = false;
    
    if(EndFlare)
	EndFlare.enabled = false;
	        
	if(AddSourceLight)
	StartPoint.GetComponent.<Light>().enabled = false;
	
	if(AddEndLight)
	 if(EndFlare){
	 EndFlare.GetComponent.<Light>().enabled = false;	
	 }	 
   }//end Laser On   
   
  }//end Update
  
  
  /////Icon
    function OnDrawGizmos () {
		Gizmos.DrawIcon(transform.position, "LaserIcon.psd", true);
	}