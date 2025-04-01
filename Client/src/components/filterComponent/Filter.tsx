import Select from "react-select";

interface FilterProps {
  data: { value: string | number; label: string }[];
  state: string[];
  stateAction: (values: string[]) => void;
  placeholder: string;
}

function Filter(props: FilterProps) {
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
    multiValue: (base: any) => ({
      ...base,
      backgroundColor: "rgb(255, 187, 0)",
      color: "black",
    }),
    multiValueLabel: (base: any) => ({
      ...base,
      color: "black",
      outline: "0",
    }),
    multiValueRemove: (base: any) => ({
      ...base,
      color: "black",
      "&:hover": {
        backgroundColor: "#FF4500",
        color: "#FFF",
      },
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
  return (
    <Select
      isMulti
      options={props.data}
      value={props.data.filter((o: any) => props.state.includes(o.value))}
      styles={customStyles}
      onChange={(e) => {
        props.stateAction(
          e
            .map((o) => o.value)
            .filter((value): value is string => typeof value === "string")
        );
      }}
      placeholder={props.placeholder}
    />
  );
}

export default Filter;
