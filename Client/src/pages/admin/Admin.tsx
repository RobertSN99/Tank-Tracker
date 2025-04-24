import { useNavigate } from "react-router-dom";
import "./Admin.css";

function Admin() {
  const navigate = useNavigate();
  return (
    <div className="admin-container">
      <ul className="admin-links-group">
        <li className="admin-link" onClick={() => navigate("/create/tank")}>
          <div className="admin-link-title">Create Tank</div>
          <div className="admin-link-description">
            Create new tank and add it to the database.
          </div>
        </li>
        <li className="admin-link" onClick={() => navigate("/create/nation")}>
          <div className="admin-link-title">Create Nation</div>
          <div className="admin-link-description">
            Create new nation and add it to the database.
          </div>
        </li>
        <li className="admin-link" onClick={() => navigate("/create/class")}>
          <div className="admin-link-title">Create Class</div>
          <div className="admin-link-description">
            Create new class and add it to the database.
          </div>
        </li>
        <li className="admin-link" onClick={() => navigate("/create/status")}>
          <div className="admin-link-title">Create Status</div>
          <div className="admin-link-description">
            Create new status and add it to the database.
          </div>
        </li>
      </ul>
    </div>
  );
}

export default Admin;
