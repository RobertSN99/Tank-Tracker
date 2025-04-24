import axios from "axios";
import React, { useEffect, useState } from "react";
import Select from "react-select";
import BackButton from "../../../components/backButton/BackButton";
import "../Create.css";

type Option = {
  value: number;
  label: string;
};

function CreateTank() {
  const [formData, setFormData] = useState({
    name: "",
    tier: 1,
    rating: 0,
    imageURL: "",
    nationId: 0,
    tankClassId: 0,
    statusId: 0,
  });

  const [nations, setNations] = useState<Option[]>([]);
  const [tankClasses, setTankClasses] = useState<Option[]>([]);
  const [statuses, setStatuses] = useState<Option[]>([]);
  const [loading, setLoading] = useState(true);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.post("https://localhost:7018/api/tank", formData, {
        withCredentials: true,
      });
      alert("Tank created successfully!");
    } catch (err: any) {
      console.error(err);
      err?.response?.status == 401 && (window.location.href = "/unauthorized");
      alert(err?.response?.data?.message || "Failed to create tank.");
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]:
        name === "tier" || name === "rating" || name.includes("Id")
          ? Number(value)
          : value,
    }));
  };

  const customStyles = {
    control: (base: any, state: { isFocused: boolean }) => ({
      ...base,
      backgroundColor: "var(--background-color)",
      borderOpacity: "0.5",
      borderColor: state.isFocused
        ? "var(--foreground-color)"
        : "var(--primary-text-color)",
      color: "var(--primary-text-color)",
      "&:hover": {
        borderColor: "var(--foreground-color)",
      },
      textAlign: "center",
    }),
    menu: (base: any) => ({
      ...base,
      backgroundColor: "var(--background-color)",
      borderRadius: "0.5rem",
    }),
    option: (base: any) => ({
      ...base,
      background: "var(--background-color)",
      color: "var(--primary-text-color)",
      "&:hover": {
        background: "rgb(255, 187, 0)",
        color: "black",
      },
    }),
    singleValue: (base: any) => ({
      ...base,
      backgroundColor: "rgb(255, 187, 0)",
      color: "black",
    }),
    input: (base: any) => ({
      ...base,
      color: "var(--primary-text-color)",
      outline: 0,
      "input:focus": {
        boxShadow: "none",
        borderRadius: 0,
      },
    }),
  };

  useEffect(() => {
    const fetchOptions = async () => {
      try {
        const [nRes, cRes, sRes] = await Promise.all([
          axios.get("https://localhost:7018/api/nation/all"),
          axios.get("https://localhost:7018/api/tankClass/all"),
          axios.get("https://localhost:7018/api/status/all"),
        ]);
        setNations(
          nRes.data.data.map((n: any) => ({ value: n.id, label: n.name }))
        );
        setTankClasses(
          cRes.data.data.map((tc: any) => ({ value: tc.id, label: tc.name }))
        );
        setStatuses(
          sRes.data.data.map((s: any) => ({ value: s.id, label: s.name }))
        );
      } catch (err) {
        alert("Failed to load dropdown options.");
      } finally {
        setLoading(false);
      }
    };

    fetchOptions();
  }, []);

  if (loading) return <div className="p-4">Loading...</div>;

  return (
    <div className="tank-page-container">
      <h1>Create New Tank</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="name"
          placeholder="Tank Name"
          value={formData.name}
          onChange={handleChange}
          required
        />
        <label htmlFor="tier">Tier:</label>
        <input
          type="number"
          name="tier"
          value={formData.tier}
          onChange={handleChange}
          required
          min={1}
          max={10}
        />
        <label htmlFor="rating">Rating:</label>
        <input
          type="number"
          name="rating"
          value={formData.rating}
          onChange={handleChange}
          required
          min={0}
          max={5}
          step={0.5}
        />
        <input
          type="text"
          name="imageURL"
          value={formData.imageURL}
          onChange={handleChange}
          placeholder="Image URL..."
        />
        <div className="options-selection-group">
          <Select
            options={tankClasses}
            value={tankClasses.find((tc) => tc.value === formData.tankClassId)}
            onChange={(option) =>
              setFormData({ ...formData, tankClassId: option?.value || 0 })
            }
            placeholder="Select Tank Class"
            styles={customStyles}
          />
          <Select
            options={nations}
            value={nations.find((n) => n.value === formData.nationId)}
            onChange={(option) =>
              setFormData({ ...formData, nationId: option?.value || 0 })
            }
            placeholder="Select Nation"
            styles={customStyles}
          />
          <Select
            options={statuses}
            value={statuses.find((s) => s.value === formData.statusId)}
            onChange={(option) =>
              setFormData({ ...formData, statusId: option?.value || 0 })
            }
            placeholder="Select Status"
            styles={customStyles}
          />
        </div>
        <button className="creation-btn" type="submit">
          Create Tank
        </button>
      </form>
      <BackButton />
    </div>
  );
}

export default CreateTank;
