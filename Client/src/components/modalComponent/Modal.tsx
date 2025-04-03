import { useState, useEffect } from "react";
import "./Modal.css";

interface ModalProps {
  type: string;
  onClose: () => void;
  tiers?: string[];
  classes?: string[];
  nations?: string[];
  statuses?: string[];
}

function Modal({
  type,
  onClose,
  tiers,
  classes,
  nations,
  statuses,
}: ModalProps) {
  const [formData, setFormData] = useState<any>({
    name: "",
    tier: "",
    class: "",
    nation: "",
    status: "",
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    type == "Tank"
      ? console.log(formData)
      : console.log({ name: formData.name });

    // Logic to handle API request
    // try {
    //   const response = await fetch("https://your-api-endpoint.com/endpoint", {
    //     method: "POST",
    //     headers: {
    //       "Content-Type": "application/json",
    //       // Add authorization headers if needed
    //     },
    //     body: JSON.stringify(formData),
    //   });

    //   if (!response.ok) {
    //     throw new Error("Failed to create the resource");
    //   }

    //   const result = await response.json();
    //   console.log("Success:", result);
    //   onClose(); // Close modal on successful form submission
    // } catch (error) {
    //   console.error("Error:", error);
    //   // Optionally handle errors
    // }
  };

  // Handle input change
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prevState: any) => ({
      ...prevState,
      [name]: value,
    }));
  };

  useEffect(() => {
    const handleEscape = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        onClose();
      }
    };
    document.addEventListener("keydown", handleEscape);
    return () => document.removeEventListener("keydown", handleEscape);
  }, [onClose]);

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-container" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>
          &times;
        </button>
        <h2>Create {type.charAt(0).toUpperCase() + type.slice(1)}</h2>
        <form onSubmit={handleSubmit}>
          {type === "Tank" ? (
            <>
              <input
                type="text"
                name="name"
                placeholder="Tank Name"
                value={formData.name}
                onChange={handleChange}
                required
              />
              <select
                name="tier"
                value={formData.tier}
                onChange={handleChange}
                required
              >
                <option value="">Select Tier</option>
                {tiers?.map((tier, index) => (
                  <option key={index} value={tier}>
                    {tier}
                  </option>
                ))}
              </select>
              <select
                name="class"
                value={formData.class}
                onChange={handleChange}
                required
              >
                <option value="">Select Class</option>
                {classes?.map((classItem, index) => (
                  <option key={index} value={classItem}>
                    {classItem}
                  </option>
                ))}
              </select>
              <select
                name="nation"
                value={formData.nation}
                onChange={handleChange}
                required
              >
                <option value="">Select Nation</option>
                {nations?.map((nation, index) => (
                  <option key={index} value={nation}>
                    {nation}
                  </option>
                ))}
              </select>
              <select
                name="status"
                value={formData.status}
                onChange={handleChange}
                required
              >
                <option value="">Select Status</option>
                {statuses?.map((status, index) => (
                  <option key={index} value={status}>
                    {status}
                  </option>
                ))}
              </select>
            </>
          ) : (
            <input
              type="text"
              name="name"
              placeholder={`${type} Name`}
              value={formData.name}
              onChange={handleChange}
              required
            />
          )}
          <button type="submit">Create</button>
        </form>
      </div>
    </div>
  );
}

export default Modal;
