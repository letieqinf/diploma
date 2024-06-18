import "./CustomCheckbox.css";

function CustomCheckbox(props: { label: string, checked: boolean, onClick: any }) {
    return (
        <>
            <input type="checkbox" id="is-organization-exist-cb" className="custom-checkbox" defaultChecked={props.checked} onClick={() => props.onClick()} />
            <label htmlFor="is-organization-exist-cb">{ props.label }</label>
        </>
    );
}

CustomCheckbox.defaultProps = {
    checked: false
};

export default CustomCheckbox;