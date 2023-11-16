using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using Unity.Mathematics;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Google.XR.ARCoreExtensions.Samples.Geospatial;

public class GPSData : MonoBehaviour
{
    public static GPSData instance;
    [SerializeField] AREarthManager earthManager;
    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }
    [Serializable]
    public struct EarthPosition
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
    }
    [SerializeField]  ARAnchorManager aRAnchorManager;

   ARAnchorManager AR = new ARAnchorManager();



    public Text latitudeText; 
    public Text longitudeText; 
    public Text songVolumeText;
    public TextMeshProUGUI closestAttractionText;
    public TextMeshProUGUI closestAttractionDistanceText;
    public TextMeshProUGUI closestAttractionSongText;
    public TextMeshProUGUI aboutText;
    double latitude = Input.location.lastData.latitude;
    double longitude = Input.location.lastData.longitude;
    public TextMeshProUGUI textObject;
    public Text warningText;
    public GameObject prefab;
    public GameObject geoReference;
    public ARAnchorManager AnchorManager;
    public AudioSource song;
    public GameObject objekti;

    public GameObject GeospatialAssetPrefab;

    public Button linkButton;

    public struct Attraction
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Song { get; set; }
        public string Artist { get; set; }
        public string Link { get; set; }
        public string About { get; set; }

        public Attraction(string name, double lat, double lon, string song, string artist, string link, string about)
        {
            Name = name;
            Latitude = lat;
            Longitude = lon;
            Song = song;
            Artist = artist;
            Link = link;
            About = about;
        }
    }


    public static List<Attraction> attractions = new List<Attraction> 
        {
            new Attraction("Diocletian's Palace", 43.507818, 16.440835, "Nima Splita do Splita", "Tereza Kesovija", "https://www.youtube.com/watch?v=tt7k8NW4qSM", "A UNESCO World Heritage site, former palace of Roman Emperor Diocletian."),
new Attraction("Marjan Hill", 43.508115660657118, 16.441144633702859, "Marjane, Marjane", "Local Folk Music", "https://www.youtube.com/watch?v=MXdltQeDFu4", "Marjan Hill, often referred to as the lungs of Split, is a scenic park and nature reserve. It offers panoramic views of the city and the Adriatic Sea, making it a popular spot for hiking, jogging, and enjoying the outdoors. Visitors can explore lush Mediterranean forests, historic hermitages, and the famous Marjan Staircase for a truly serene experience."),
new Attraction("Riva Promenade", 43.507189552469633, 16.438759311153763, "Dalmatinac", "Mate Mišo Kovaè", "https://www.youtube.com/watch?v=2iPhtt46qAQ", "The Riva Promenade is a lively waterfront area in the heart of Split. With a backdrop of historic architecture and palm trees, it's a bustling hub filled with cafes, restaurants, and shops. It's a perfect place to enjoy a leisurely stroll, soak in the Mediterranean atmosphere, and take in views of the sparkling sea."),
new Attraction("Split Harbor", 43.5048055494502, 16.44095735169184, "Sve Bi Seke Ljubile Mornare", "Magazin", "https://youtu.be/nfNcGtpWhbI?si=9etH8i4olI7MBj_4", "Split Harbor is a picturesque waterfront area, offering a charming blend of historic charm and maritime beauty. With bobbing boats and a stunning view of the Adriatic Sea, it's an ideal spot for a relaxing walk. Enjoy the sounds of traditional Dalmatian music as you explore the harbor's vibrant atmosphere, dotted with cafes and local vendors."),
    new Attraction("Baèvice", 43.502078752088444, 16.447066093709502, "Idemo na Baèvice", "Stividen", "https://www.youtube.com/watch?v=hjt3Z482KDc&ab_channel=CroatiaRecords", "Bacvice Beach is one of the most famous beaches in Split. Its shallow, sandy shores make it a popular choice for swimming and beach games. The beach is surrounded by cafes and restaurants, offering a vibrant atmosphere, especially during the summer. It's an ideal spot for both locals and tourists to enjoy the sun and sea."),
new Attraction("Mestrovic Gallery", 43.504841909349416, 16.4176622023504, "Galerija", "Corona", "https://youtu.be/kyP1OUG4zw8?si=TWD0q7bkrsn-p8kA", "The former home of Ivan Mestrovic, showcasing his sculptures and artwork."),
new Attraction("Brda", 43.52386404265241, 16.46852667455853, "Brda", "TTM", "https://youtu.be/yVIhxBr3lsk?si=rXfFAJyr2m4BRtdf", " Brda is a neighborhood in Split known for its residential charm and panoramic views of the city and the sea. It offers a peaceful escape from the city center while being in close proximity to local amenities. Brda's elevated location and quiet streets make it an attractive residential area, with an authentic ambiance that reflects everyday life in Split."),
new Attraction("FESB", 43.510704120529894, 16.468476406743807, "Alles probiert", "Raf Camora", "https://youtu.be/BixqbSRjY2Y?si=brwFfNMMeog5_VCx", "Faculty of Electrical Engineering, Mechanical Engineering, and Naval Architecture, is a distinguished academic institution within the University of Split. It is renowned for its excellence in engineering education and research, offering a diverse range of programs. "),
new Attraction("Marina Kasuni", 43.50617696895793, 16.400555659879004, "Dalmatinka", "Jelena Rozga", "https://youtu.be/Ghj8oRCRhBY?si=8-BvBbxDHK3U44UG", "Marina Kasjuni is a picturesque marina nestled on the scenic coast of Split. With its tranquil atmosphere and crystal-clear waters, it's a favored spot for those seeking a peaceful retreat. Visitors can relax by the sea, take in breathtaking views of the Adriatic, and explore the nearby pine-scented forests. The marina is a hidden gem for those looking to escape the city's hustle and bustle."),
new Attraction("The Shipyard", 43.52527321533893, 16.441016266679448, "Kako æu joj reæ da varin", "Sveti Florijan", "https://youtu.be/M85LQ2qjxU0?si=wDv-d17FR9E80k_T", "The Shipyard in Split has a rich history as a hub for shipbuilding and maritime activities. It has been involved in constructing a wide range of vessels, from cargo ships to military vessels and luxury yachts. The shipyard has contributed to the development of Split as a major port city on the Adriatic coast and has been instrumental in the city's industrial and economic growth."),
new Attraction("Poljud", 43.518491512221466, 16.431845902428478, "Hajduèka", "Oliver Dragojeviæ", "https://youtu.be/AcZVMNeaFLk?si=VPBoO_WMomgXSFl4", "Poljud Stadium in Split holds a storied history deeply ingrained in the city's cultural and sporting identity. Serving as a focal point for athletic events and communal gatherings, Poljud Stadium has played a pivotal role in shaping Split's landscape."),
new Attraction("Žnjan Beach", 43.502816754580756, 16.476278108964216, "Èa æe mi Copacabana", "Oliver Dragojeviæ", "https://www.youtube.com/watch?v=s6dyCA5JKSk&ab_channel=CroatiaRecords", "This pebble beach is characterized by crystal-clear waters and a scenic backdrop of the Adriatic Sea. Known for its family-friendly atmosphere, Žnjan Beach offers a range of facilities, including beach bars, restaurants, and water sports activities, making it a popular choice for both relaxation and recreation. ") 
            // Add more attractions here
        };


    public void AddAttraction(string name, double lat, double lon, string song, string artist, string link, string about)
    {
        
        Attraction newAttraction = new Attraction(name, lat, lon, song, artist, link, about);
        attractions.Add(newAttraction);
    }


    public void Start()
    {

        
        if (Input.location.isEnabledByUser)
        {
            
            Input.location.Start();
        }
        else
        {
            warningText.text = "Location services are not enabled. Please enable them in your device settings.";
            latitudeText.text = "not enabled.";
            longitudeText.text = "not enabled.";
        }

        SetRandomColors();

        Button btn = linkButton.GetComponent<Button>();
        btn.onClick.AddListener(LinkOpen);


        foreach (Attraction attraction in attractions)
        {
            ARGeospatialAnchor anchor = AnchorManager.AddAnchor(attraction.Latitude, attraction.Longitude, 89, Quaternion.identity);
            GameObject obj = Instantiate(GeospatialAssetPrefab, anchor.transform);
            obj.name = "klon";
        }
        

        

        song = GetComponent<AudioSource>();
        song.Play();

    }
    

    void Update()
    {

        if (Input.location.status == LocationServiceStatus.Running)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            latitudeText.text = latitude.ToString("0.0000");
            longitudeText.text = longitude.ToString("0.0000");
            warningText.text = "";
        }
        else
        {
            warningText.text = "Location services are not available.";
            latitudeText.text = "not available.";
            longitudeText.text = "not available.";
            latitude = 0;
            longitude = 0;
            
        }

        
        song.volume = (float)VolumeControl(CalculateHaversineDistance(latitude, longitude, FindClosestAttraction(attractions, latitude, longitude).Latitude, FindClosestAttraction(attractions, latitude, longitude).Longitude));

        songVolumeText.text = "Volume: " + VolumeControl(CalculateHaversineDistance(latitude, longitude, FindClosestAttraction(attractions, latitude, longitude).Latitude, FindClosestAttraction(attractions, latitude, longitude).Longitude)).ToString("0") + "%";
        closestAttractionText.text = FindClosestAttraction(attractions, latitude, longitude).Name;
        closestAttractionSongText.text= FindClosestAttraction(attractions, latitude, longitude).Song + " - " + FindClosestAttraction(attractions, latitude, longitude).Artist;
        closestAttractionDistanceText.text = CalculateHaversineDistance(latitude, longitude, FindClosestAttraction(attractions, latitude, longitude).Latitude, FindClosestAttraction(attractions, latitude, longitude).Longitude).ToString("0.000") + " km";
        aboutText.text = FindClosestAttraction(attractions, latitude, longitude).About;

        if (textObject != null)
        {
            textObject.text = "";

            List<AttractionWithDistance> attractionsWithinRadius = FindAttractionsWithinRadius(attractions, latitude, longitude, 15);

            for (int i = 0; i < attractionsWithinRadius.Count; i++)
            {
                AttractionWithDistance nearAttraction = attractionsWithinRadius[i];
                string line = $"{i + 1}. {nearAttraction.Attraction.Name} - {nearAttraction.Distance.ToString("0.0")} km\n";
                textObject.text += line;
            }
        }

    }

    static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        
        double earthRadiusKm = 6371.0;

        double dLat = DegreeToRadians(lat2 - lat1);
        double dLon = DegreeToRadians(lon2 - lon1);

        double a = Mathf.Sin((float)(dLat / 2)) * Mathf.Sin((float)(dLat / 2)) +
                   Mathf.Cos((float)(DegreeToRadians(lat1))) * Mathf.Cos((float)(DegreeToRadians(lat2))) *
                   Mathf.Sin((float)(dLon / 2)) * Mathf.Sin((float)(dLon / 2));

        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));
        double distance = earthRadiusKm * c;

        return distance;
    }

    static double DegreeToRadians(double degree)
    {
        return degree * Mathf.PI / 180.0;
    }

    public static Attraction FindClosestAttraction(List<Attraction> attractions, double myLatitude, double myLongitude)
    {
        if (attractions.Count == 0)
        {
            Debug.Log("The attractions list is empty.");
        }

        double minDistance = double.MaxValue;
        Attraction closestAttraction = attractions[0]; 

        foreach (Attraction attraction in attractions)
        {
            double distance = CalculateHaversineDistance(myLatitude, myLongitude, attraction.Latitude, attraction.Longitude);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestAttraction = attraction;
            }
        }

        return closestAttraction;
    }

    public class AttractionWithDistance
    {
        public Attraction Attraction { get; set; }
        public double Distance { get; set; }
    }


    public static List<AttractionWithDistance> FindAttractionsWithinRadius(List<Attraction> attractions, double myLatitude, double myLongitude, double maxDistanceInKm)
    {
        if (attractions.Count == 0)
        {
            Debug.Log("The attractions list is empty.");
            return new List<AttractionWithDistance>(); // Return an empty list when no attractions are available.
        }

        List<AttractionWithDistance> attractionsWithDistances = new List<AttractionWithDistance>();

        foreach (Attraction attraction in attractions)
        {
            double distance = CalculateHaversineDistance(myLatitude, myLongitude, attraction.Latitude, attraction.Longitude);

            if (distance <= maxDistanceInKm)
            {
                attractionsWithDistances.Add(new AttractionWithDistance
                {
                    Attraction = attraction,
                    Distance = distance
                });
            }
        }

        attractionsWithDistances.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        return attractionsWithDistances;
    }

    public static double Map(double value, double fromLow, double fromHigh, double toLow, double toHigh)
    {
        return (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow) + toLow;
    }

    public static double VolumeControl(double distance)
    {
        if (distance > 3)
        {
            return 50;
        }
        else
        {
            return Map(distance, 3, 0, 50, 100);
        }
    }

    void SetRandomColors()
    {
        if (objekti != null)
        {
            foreach (Transform child in objekti.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                }
            }
        }
    }


    void LinkOpen()
    {
        Application.OpenURL(FindClosestAttraction(attractions, latitude, longitude).Link);
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }
}

