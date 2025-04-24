import React, { useEffect, useState } from "react";
import axios from "axios";
import Select from "react-select";
import { useParams } from "react-router-dom";
import "./TankId.css";
import { useAuth } from "../../contexts/AuthContext";
import BackButton from "../../components/backButton/BackButton";
import { IoIosStar, IoIosStarHalf, IoIosStarOutline } from "react-icons/io";

type Option = {
  value: number;
  label: string;
};

function TankId() {
  const { id } = useParams<{ id: string }>();
  const [formData, setFormData] = useState({
    name: "",
    tier: 1,
    rating: 1,
    imageURL: "",
    nationId: 0,
    tankClassId: 0,
    statusId: 0,
  });
  const [editMode, setEditMode] = useState<boolean>(false);

  const { user } = useAuth();
  const isAdmin =
    user?.roles?.includes("Administrator") ||
    user?.roles?.includes("Moderator") ||
    false;

  const [nations, setNations] = useState<Option[]>([]);
  const [tankClasses, setTankClasses] = useState<Option[]>([]);
  const [statuses, setStatuses] = useState<Option[]>([]);
  const [loading, setLoading] = useState(true);

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

  const handleSave = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.patch(`https://localhost:7018/api/tank/id/${id}`, formData, {
        withCredentials: true,
      });
      console.log(formData);
      alert("Tank updated successfully!");
      window.location.reload();
    } catch (err: any) {
      console.error(err);
      err?.response?.status == 401 && (window.location.href = "/unauthorized");
      alert(err?.response?.data?.message || "Failed to update tank.");
    }
  };

  const handleDelete = async () => {
    try {
      await axios.delete(`https://localhost:7018/api/tank/id/${id}`, {
        withCredentials: true,
      });
      alert("Tank deleted successfully!");
    } catch (err: any) {
      console.error(err);
      err?.response?.status == 401 && (window.location.href = "/unauthorized");
      alert(err?.response?.data?.message || "Failed to delete tank.");
    }
  };

  const customStyles = {
    control: (base: any, state: { isFocused: any }) => ({
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

  const colorFromRating = (rating: number) => {
    if (rating === 0) return "no-rating";
    if (rating < 2) return "terrible-rating";
    if (rating < 3) return "bad-rating";
    if (rating < 4) return "ok-rating";
    if (rating < 4.5) return "good-rating";
    if (rating < 5) return "excellent-rating";
    if (rating === 5) return "epic-rating";
    return undefined;
  };

  const renderStars = (rating: number) => {
    const fullStars = Math.floor(rating);
    const halfStars = rating % 1 >= 0.5 ? 1 : 0;
    const emptyStars = 5 - fullStars - halfStars;

    return (
      <span className={`tank-card-rating ${colorFromRating(rating)}`}>
        {Array(fullStars)
          .fill(null)
          .map((_, i) => (
            <IoIosStar key={`full-${i}`} />
          ))}
        {halfStars === 1 && <IoIosStarHalf key="half" />}
        {Array(emptyStars)
          .fill(null)
          .map((_, i) => (
            <IoIosStarOutline key={`empty-${i}`} />
          ))}
      </span>
    );
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [tankRes, nRes, cRes, sRes] = await Promise.all([
          axios.get(`https://localhost:7018/api/tank/id/${id}`),
          axios.get("https://localhost:7018/api/nation/all"),
          axios.get("https://localhost:7018/api/tankClass/all"),
          axios.get("https://localhost:7018/api/status/all"),
        ]);

        const tank = tankRes.data.data;
        setFormData({
          name: tank.name,
          tier: tank.tier,
          rating: tank.rating,
          imageURL: tank.imageURL,
          nationId: tank.nationId,
          tankClassId: tank.tankClassId,
          statusId: tank.statusId,
        });

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
        alert("Failed to load tank data or dropdowns.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  if (loading) return <div className="p-4">Loading...</div>;

  return (
    <div className="tank-page-container">
      {isAdmin && editMode ? (
        <>
          <h1>Edit Tank</h1>
          <form onSubmit={handleSave}>
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
              placeholder="ImageURL..."
              value={formData.imageURL}
              onChange={handleChange}
              maxLength={256}
            />
            <div className="options-selection-group">
              <Select
                options={tankClasses}
                value={tankClasses.find(
                  (tc) => tc.value === formData.tankClassId
                )}
                onChange={(option) =>
                  setFormData({
                    ...formData,
                    tankClassId: option?.value || 0,
                  })
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
              Save Changes
            </button>
          </form>
        </>
      ) : (
        <>
          <h1>
            <span>{renderStars(formData.rating)}</span>
            <span className={colorFromRating(formData.rating)}>
              {formData.name}
            </span>
          </h1>
          <div className="tank-details">
            <img src={formData.imageURL} className="tank-image" alt="" />
            <p className="detail-item">
              <span>Tier:</span> {formData.tier}
            </p>
            <p className="detail-item">
              <span>Nation:</span>{" "}
              {nations.find((n) => n.value === formData.nationId)?.label}
            </p>
            <p className="detail-item">
              <span>Tank Class:</span>{" "}
              {
                tankClasses.find((tc) => tc.value === formData.tankClassId)
                  ?.label
              }
            </p>
            <p className="detail-item">
              <span>Status:</span>{" "}
              {statuses.find((s) => s.value === formData.statusId)?.label}
            </p>
          </div>
        </>
      )}
      {isAdmin && !editMode && (
        <div className="tankid-admin-buttons">
          <button
            className="tankid-edit-btn"
            onClick={() => setEditMode(!editMode)}
          >
            Edit
          </button>
          <button className="tankid-delete-btn" onClick={handleDelete}>
            Delete
          </button>
        </div>
      )}
      <BackButton />
    </div>
  );
}

export default TankId;
