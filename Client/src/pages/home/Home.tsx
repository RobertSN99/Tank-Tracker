import { useEffect, useState } from "react";
import "./Home.css";
import Filter from "../../components/filterComponent/Filter";
import TankContainer from "../../components/tankContainer/TankContainer";
import axios from "axios";

interface Tank {
  id: number;
  name: string;
  tier: number;
  rating: number;
  imageURL: string;
  tankClassName: string;
  statusName: string;
  nationName: string;
}

interface FilterOption {
  value: string;
  label: string;
}

function Home() {
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [debouncedSearch, setDebouncedSearch] = useState<string>("");
  const [selectedNations, setSelectedNations] = useState<string[]>([]);
  const [selectedClasses, setSelectedClasses] = useState<string[]>([]);
  const [selectedTiers, setSelectedTiers] = useState<string[]>([]);
  const [selectedStatuses, setSelectedStatuses] = useState<string[]>([]);
  const [selectedRatings, setSelectedRatings] = useState<string[]>([]);
  const [tanks, setTanks] = useState<Tank[]>([]);

  const [nationOptions, setNationOptions] = useState<FilterOption[]>([]);
  const [tankClassOptions, setTankClassOptions] = useState<FilterOption[]>([]);
  const [statusOptions, setStatusOptions] = useState<FilterOption[]>([]);

  const [pageNumber, setPageNumber] = useState<number>(1);
  const [totalCount, setTotalCount] = useState<number>(0);
  const totalPages = Math.ceil(totalCount / 6);

  useEffect(() => {
    const timeout = setTimeout(() => {
      setDebouncedSearch(searchTerm);
      setPageNumber(1);
    }, 500);
    return () => clearTimeout(timeout);
  }, [searchTerm]);

  const handleReset = () => {
    setSearchTerm("");
    setSelectedNations([]);
    setSelectedClasses([]);
    setSelectedTiers([]);
    setSelectedStatuses([]);
    setSelectedRatings([]);
    setPageNumber(1);
  };

  const buildQueryParams = () => {
    const params = new URLSearchParams();
    params.append("pageNumber", pageNumber.toString());
    params.append("pageSize", "6");

    if (debouncedSearch) {
      params.append("searchTerm", debouncedSearch);
    }
    selectedNations.forEach((nation) => params.append("nationNames", nation));
    selectedClasses.forEach((cls) => params.append("tankClassNames", cls));
    selectedTiers.forEach((tier) => params.append("tiers", tier));
    selectedStatuses.forEach((status) => params.append("statusNames", status));
    selectedRatings.forEach((rating) => params.append("ratings", rating));
    return params.toString();
  };

  useEffect(() => {
    const fetchTanks = async () => {
      try {
        const queryParams = buildQueryParams();
        // Use the queryParams in the API call, and sort them by latest created descending
        const response = await axios.get(
          `https://localhost:7018/api/tank/all?${queryParams}&sortBy=CreatedAt&sortOrder=desc`
        );
        const items = response.data.data.items;
        const mappedTanks: Tank[] = items.map((item: any) => ({
          id: item.id,
          name: item.name,
          tier: item.tier,
          rating: item.rating,
          imageURL: item.imageURL,
          nationName: item.nationName,
          tankClassName: item.tankClassName,
          statusName: item.statusName,
        }));
        setTanks(mappedTanks);
        setTotalCount(response.data.data.totalCount);
      } catch (err) {
        console.error("Error fetching tanks:", err);
        setTanks([]);
        setTotalCount(0);
      }
    };

    const fetchFilterOptions = async () => {
      try {
        const [nationRes, classRes, statusRes] = await Promise.all([
          axios.get("https://localhost:7018/api/nation/all"),
          axios.get("https://localhost:7018/api/tankClass/all"),
          axios.get("https://localhost:7018/api/status/all"),
        ]);

        setNationOptions(
          nationRes.data.data.map((n: any) => ({
            value: n.name,
            label: n.name,
          }))
        );

        setTankClassOptions(
          classRes.data.data.map((c: any) => ({
            value: c.name,
            label: tankClassMap[c.name],
          }))
        );

        setStatusOptions(
          statusRes.data.data.map((s: any) => ({
            value: s.name,
            label: s.name,
          }))
        );
      } catch (err) {
        console.error("Error fetching filter options:", err);
      }
    };

    fetchTanks();
    fetchFilterOptions();
  }, [
    pageNumber,
    selectedNations,
    selectedClasses,
    selectedTiers,
    selectedStatuses,
    selectedRatings,
    debouncedSearch,
  ]);

  const tankClassMap: { [key: string]: string } = {
    HT: "Heavy Tank",
    MT: "Medium Tank",
    LT: "Light Tank",
    TD: "Tank Destroyer",
    SPG: "Self-Propelled Gun",
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
              data={nationOptions}
              state={selectedNations}
              stateAction={(vals) => {
                setSelectedNations(vals), setPageNumber(1);
              }}
              placeholder="Select Nations"
            />
            <Filter
              data={tankClassOptions}
              state={selectedClasses}
              stateAction={(vals) => {
                setSelectedClasses(vals), setPageNumber(1);
              }}
              placeholder="Select Classes"
            />
            <Filter
              data={[
                { value: "1", label: "Tier I" },
                { value: "2", label: "Tier II" },
                { value: "3", label: "Tier III" },
                { value: "4", label: "Tier IV" },
                { value: "5", label: "Tier V" },
                { value: "6", label: "Tier VI" },
                { value: "7", label: "Tier VII" },
                { value: "8", label: "Tier VIII" },
                { value: "9", label: "Tier IX" },
                { value: "10", label: "Tier X" },
              ]}
              state={selectedTiers}
              stateAction={(vals) => {
                setSelectedTiers(vals), setPageNumber(1);
              }}
              placeholder="Select Tiers"
            />
            <Filter
              data={statusOptions}
              state={selectedStatuses}
              stateAction={(vals) => {
                setSelectedStatuses(vals), setPageNumber(1);
              }}
              placeholder="Select Statuses"
            />
            <Filter
              data={[
                { value: "1.0", label: "1 Star" },
                { value: "1.5", label: "1.5 Stars" },
                { value: "2.0", label: "2 Stars" },
                { value: "2.5", label: "2.5 Stars" },
                { value: "3.0", label: "3 Stars" },
                { value: "3.5", label: "3.5 Stars" },
                { value: "4.0", label: "4 Stars" },
                { value: "4.5", label: "4.5 Stars" },
                { value: "5.0", label: "5 Stars" },
              ]}
              state={selectedRatings}
              stateAction={(vals) => {
                setSelectedRatings(vals), setPageNumber(1);
              }}
              placeholder="Select Ratings"
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
        <TankContainer
          tanks={tanks}
          currentPage={pageNumber}
          onPageChange={setPageNumber}
          totalPages={totalPages}
        />
      </section>
    </main>
  );
}

export default Home;
