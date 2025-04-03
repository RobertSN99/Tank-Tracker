import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./TankId.css";
import { CiEdit } from "react-icons/ci";
import { HiArrowLongLeft } from "react-icons/hi2";

interface Tank {
  id: string;
  name: string;
  tier: string;
  class: string;
  nation: string;
  status: string;
}

// Placeholder tank data (should be replaced with API call later)
const mockTanks: Tank[] = [
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
];

function TankId() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [tank, setTank] = useState<Tank | null>(null);
  const [editingField, setEditingField] = useState<keyof Tank | null>(null);
  const [editedTank, setEditedTank] = useState<Tank | null>(null);

  // Placeholder options
  const tiers = ["Tier 1", "Tier 2", "Tier 3", "Tier 4"];
  const classes = ["Light Tank", "Medium Tank", "Heavy Tank", "Tank Destroyer"];
  const nations = ["Czech", "Germany", "France", "USA", "UK"];
  const statuses = ["Tech Tree", "Premium", "Special", "Event"];

  useEffect(() => {
    // Simulate fetching tank data from an API by finding it in mockTanks
    const foundTank = mockTanks.find((t) => t.id === id);
    if (foundTank) {
      setTank(foundTank);
      setEditedTank(foundTank);
    }
  }, [id]);

  const handleEditChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    field: keyof Tank
  ) => {
    if (editedTank) {
      setEditedTank({ ...editedTank, [field]: e.target.value });
    }
  };

  const handleSave = () => {
    console.log("Save changes", editedTank);
    setEditingField(null);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSave();
    }
  };

  if (!tank) return <p>Loading...</p>;

  return (
    <div className="tank-page-container">
      <div className="tank-details">
        {["name", "tier", "class", "nation", "status"].map((field) => (
          <p key={field}>
            <strong>{field.charAt(0).toUpperCase() + field.slice(1)}:</strong>{" "}
            {editingField === field ? (
              field === "tier" ||
              field === "class" ||
              field === "nation" ||
              field === "status" ? (
                <select
                  value={editedTank?.[field as keyof Tank]}
                  onChange={(e) => handleEditChange(e, field as keyof Tank)}
                  onBlur={handleSave}
                  autoFocus
                  className="editable-select"
                >
                  {(field === "tier"
                    ? tiers
                    : field === "class"
                    ? classes
                    : field === "nation"
                    ? nations
                    : statuses
                  ).map((option, index) => (
                    <option key={index} value={option}>
                      {option}
                    </option>
                  ))}
                </select>
              ) : (
                <input
                  type="text"
                  value={editedTank?.[field as keyof Tank]}
                  onChange={(e) => handleEditChange(e, field as keyof Tank)}
                  onBlur={handleSave}
                  onKeyDown={handleKeyDown}
                  autoFocus
                  className="editable-input"
                />
              )
            ) : (
              <>
                {tank[field as keyof Tank]}{" "}
                <span onClick={() => setEditingField(field as keyof Tank)}>
                  <CiEdit />
                </span>
              </>
            )}
          </p>
        ))}
        <div className="actions">
          <button className="back-btn" onClick={() => navigate("/home")}>
            <span>
              <HiArrowLongLeft />
            </span>{" "}
            Back to all tanks
          </button>
          <button
            className="delete-btn"
            onClick={() => console.log("Delete tank", tank.id)}
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}

export default TankId;
