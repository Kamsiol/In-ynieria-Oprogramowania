import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
/*
interface Bike {
  IDbike: number;
  nameModel: number;
  typeModel: string;
  priceModel: number;
  photoModel: string;
  availableBike: boolean;
  IDreturnLocation?: string;
}
*/

interface Bike{
  iDbike: number,
  iDmodel: number,
      nameModel: string,
      typeModel: string,
      priceModel: number,
      amountBike: number,
      availableBike: boolean
}
const BikeList = () => {
  const [bikes, setBikes] = useState<Bike[]>([]);
  const [filteredBikes, setFilteredBikes] = useState<Bike[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Filtering states
  const [searchQuery, setSearchQuery] = useState<string>("");
  const [selectedType, setSelectedType] = useState<string>("All");
  const [selectedAvailability, setSelectedAvailability] = useState<string>("All");

  useEffect(() => {
    axios.get("http://localhost:5008/api/Bikes/")
      .then((response) => {
        setBikes(response.data);
        setFilteredBikes(response.data);
        //console.log(response.data);
        setLoading(false);
      })
      .catch((error) => {
        setError("Failed to load bikes.");
        setLoading(false);
      });
  }, []);

  // Apply filters
  useEffect(() => {
    let filtered = bikes;

   /* if (selectedType !== "All") {
      filtered = filtered.filter((bike) => bike.typeModel === selectedType);
    }*/

    if (selectedAvailability !== "All") {
      const isAvailable = selectedAvailability === "Available";
      filtered = filtered.filter((bike) => bike.availableBike === isAvailable);
    }

    if (searchQuery) {
      filtered = filtered.filter((bike) =>
        bike.nameModel.toString().toLowerCase().includes(searchQuery.toLowerCase())
        //bike.iDbike.toString().includes(searchQuery)
      );
    }
/*




*/
    setFilteredBikes(filtered);
  }, [searchQuery, selectedType, selectedAvailability, bikes]);

  if (loading) return <p className="text-center text-gray-500">Loading bikes...</p>;
  if (error) return <p className="text-center text-red-500">{error}</p>;

  return (
    <div className="max-w-6xl mx-auto mt-10">
      <h2 className="text-2xl font-bold text-center mb-6">Available Bikes</h2>

      {/* Filters Section */}
      <div className="flex flex-wrap justify-center gap-4 mb-6">
        {/* Search Bar */}
        <input
          type="text"
          placeholder="Search by model..."
          className="p-2 border rounded-md shadow"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />

        {/* Type Filter */}
        <select
          className="p-2 border rounded-md shadow"
          value={selectedType}
          onChange={(e) => setSelectedType(e.target.value)}
        >
          <option value="All">All Types</option>
          <option value="Mountain">Mountain</option>
          <option value="Road">Road</option>
          <option value="Hybrid">Hybrid</option>
        </select>

        {/* Availability Filter */}
        <select
          className="p-2 border rounded-md shadow"
          value={selectedAvailability}
          onChange={(e) => setSelectedAvailability(e.target.value)}
        >
          <option value="All">All Availability</option>
          <option value="Available">Available</option>
          <option value="Not Available">Not Available</option>
        </select>
      </div>

      {/* Bike Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {filteredBikes.length > 0 ? (
          filteredBikes.map((bike) => (
            <div key={bike.iDbike} className="border p-4 rounded-lg shadow-md bg-white">
              
              <h3 className="text-lg font-semibold">Model {bike.nameModel}</h3>
              <p className="text-gray-600">{bike.typeModel}</p>
              <p className="font-bold text-blue-600">${bike.priceModel}</p>
              <p className={`mt-2 text-sm ${bike.availableBike ? "text-green-500" : "text-red-500"}`}>
                {bike.availableBike ? "Available" : "Not Available"}
              </p>
              <button
              onClick={() => navigate(`/bike/${bike.iDbike}`)}
              className="mt-3 bg-blue-500 text-white py-1 px-4 rounded hover:bg-blue-600 transition"
            >
              View Details
            </button>
            </div>
          ))
        ) : (
          <p className="text-center col-span-full text-gray-500">No bikes match your filters.</p>
        )}
      </div>
    </div>
  );
};

export default BikeList;
