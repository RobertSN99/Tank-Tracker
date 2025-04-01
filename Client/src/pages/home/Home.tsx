import { useState } from "react";
import "./Home.css";
import Filter from "../../components/filterComponent/Filter";
import TankContainer from "../../components/tankContainer/TankContainer";

function Home() {
  const [searchTerm, setSearchTerm] = useState<string>(""); // Search input state
  const [selectedNations, setSelectedNations] = useState<string[]>([]);
  const [selectedClasses, setSelectedClasses] = useState<string[]>([]);
  const [selectedTiers, setSelectedTiers] = useState<string[]>([]);
  const [selectedStatuses, setSelectedStatuses] = useState<string[]>([]);

  const handleReset = () => {
    setSearchTerm("");
    setSelectedNations([]);
    setSelectedClasses([]);
    setSelectedTiers([]);
    setSelectedStatuses([]);
  };
  const tanks = [
    {
      id: "id-example0",
      name: "Skoda T50",
      tier: "9",
      class: "Medium Tank",
      nation: "Czech",
      status: "Tech Tree",
    },
    {
      id: "id-example1",
      name: "SP I C",
      tier: "7",
      class: "Light Tank",
      nation: "Germany",
      status: "Tech Tree",
    },
    {
      id: "id-example2",
      name: "SFAC 105",
      tier: "8",
      class: "Tank Destroyer",
      nation: "France",
      status: "Premium",
    },
    {
      id: "id-example7",
      name: "Panzer V/IV",
      tier: "5",
      class: "Medium Tank",
      nation: "Germany",
      status: "Premium",
    },
    {
      id: "id-example8",
      name: "Progetto 46",
      tier: "8",
      class: "Medium Tank",
      nation: "Italy",
      status: "Premium",
    },
    {
      id: "id-example3",
      name: "113 BO",
      tier: "10",
      class: "Heavy Tank",
      nation: "China",
      status: "Collector",
    },
    {
      id: "id-example4",
      name: "Concept 1B",
      tier: "9",
      class: "Heavy Tank",
      nation: "USA",
      status: "Reward",
    },
    {
      id: "id-example5",
      name: "T95/FV4201 Chieftain",
      tier: "10",
      class: "Heavy Tank",
      nation: "UK",
      status: "Reward",
    },
    {
      id: "id-example6",
      name: "Kampfpanzer 50t",
      tier: "9",
      class: "Medium Tank",
      nation: "Germany",
      status: "Reward",
    },
  ];

  // Filtering function
  const filterTanks = () => {
    return tanks.filter((tank) => {
      const nameMatch =
        searchTerm === "" ||
        tank.name.toLowerCase().includes(searchTerm.toLowerCase());
      const nationMatch =
        selectedNations.length === 0 || selectedNations.includes(tank.nation);
      const classMatch =
        selectedClasses.length === 0 || selectedClasses.includes(tank.class);
      const tierMatch =
        selectedTiers.length === 0 ||
        selectedTiers.includes(tank.tier.toString());
      const statusMatch =
        selectedStatuses.length === 0 || selectedStatuses.includes(tank.status);

      return nameMatch && nationMatch && classMatch && tierMatch && statusMatch;
    });
  };

  return (
    <main className="home-container">
      <div className="home-title">
        <h1>
          Welcome to <span>Tank Tracker</span>!
        </h1>
        <h3>Your World of Tanks tanks manager</h3>
      </div>

      <section className="filter-container">
        <div className="filter-card">
          <h4 className="filter-card-title">Filter vehicles</h4>
          <button className="filter-reset-btn" onClick={handleReset}>
            Reset filters
          </button>
          <div className="filter-group">
            <Filter
              data={[
                { value: "USA", label: "USA" },
                { value: "Germany", label: "Germany" },
              ]}
              state={selectedNations}
              stateAction={setSelectedNations}
              placeholder="Select Nations"
            />
            <Filter
              data={[
                { value: "Light Tank", label: "Light Tank" },
                { value: "Heavy Tank", label: "Heavy Tank" },
              ]}
              state={selectedClasses}
              stateAction={setSelectedClasses}
              placeholder="Select Classes"
            />
            <Filter
              data={[
                { value: "5", label: "Tier V" },
                { value: "10", label: "Tier X" },
              ]}
              state={selectedTiers}
              stateAction={setSelectedTiers}
              placeholder="Select Tiers"
            />
            <Filter
              data={[
                { value: "Tech Tree", label: "Tech Tree" },
                { value: "Premium", label: "Premium" },
              ]}
              state={selectedStatuses}
              stateAction={setSelectedStatuses}
              placeholder="Select Statuses"
            />
          </div>
        </div>
      </section>

      {/* Tank List Section */}
      <section className="tanks-container">
        <input
          type="text"
          placeholder="Search tank by name..."
          className="search-bar"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <TankContainer tanks={filterTanks()} />
      </section>
    </main>
  );
}

export default Home;
