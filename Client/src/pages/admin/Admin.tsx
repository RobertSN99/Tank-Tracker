import { useState } from "react";
import "./Admin.css";
import Modal from "../../components/modalComponent/Modal";

function Admin() {
  const [modalType, setModalType] = useState<string | null>(null);

  // placeholder options
  const tiers = ["Tier 1", "Tier 2", "Tier 3"];
  const classes = ["Class A", "Class B", "Class C"];
  const nations = ["Nation X", "Nation Y", "Nation Z"];
  const statuses = ["Active", "Inactive", "Retired"];

  return (
    <div className="admin-container">
      <ul className="admin-links-group">
        {[
          { type: "Tank", title: "Tanks", description: "Add a new tank" },
          { type: "Nation", title: "Nations", description: "Add a new nation" },
          { type: "Class", title: "Classes", description: "Add a new class" },
          { type: "Tier", title: "Tiers", description: "Add a new tier" },
          {
            type: "Status",
            title: "Statuses",
            description: "Add a new status",
          },
        ].map(({ type, title, description }) => (
          <li
            key={type}
            className="admin-link"
            onClick={() => setModalType(type)}
          >
            <div className="admin-link-title">{title}</div>
            <div className="admin-link-description">{description}</div>
          </li>
        ))}
      </ul>
      {modalType && (
        <Modal
          type={modalType}
          onClose={() => setModalType(null)}
          tiers={modalType === "Tank" ? tiers : undefined}
          classes={modalType === "Tank" ? classes : undefined}
          nations={modalType === "Tank" ? nations : undefined}
          statuses={modalType === "Tank" ? statuses : undefined}
        />
      )}
    </div>
  );
}

export default Admin;
