import { useEffect, useRef } from "react";
import "./CustomInput.css";

function CustomInput(props: { 
    label: any, 
    collection: string, 
    name: string, 
    value: any, 
    className: string,
    onChange: any, 
    onFocus: any | undefined, 
    onBlur: any | undefined,
    disabled?: boolean
}) {
    const onLoad = useRef(false);

    useEffect(() => {        
        const changeInputPadding = () => {
            let labels = document.querySelectorAll(`.custom-input.--${ props.collection } .label`);
            let inputs = document.querySelectorAll(`.custom-input.--${ props.collection } .input`);
            
            if (labels && inputs) {
                let maxWidth = 0;
                labels.forEach(label => {
                    if (label instanceof HTMLElement) {
                        maxWidth = (label.clientWidth > maxWidth) ? label.clientWidth : maxWidth;
                    }
                });
    
                inputs.forEach(input => {
                    if (input instanceof HTMLElement) {
                        input.style.paddingLeft = `${maxWidth + 28}px`;
                    }
                });
            }
        }

        if (onLoad.current) {
            changeInputPadding();
        }
            

        onLoad.current = true;
    }, [])

    return (
        <div className={ `custom-input ${ props.className } --${ props.collection }` }>
            <label className="label" htmlFor={ props.name }>{ props.label }</label>
            <input
                type="text"
                className="input"
                name={ props.name }
                value={ props.value }
                onChange={ props.onChange }
                onFocus={ props.onFocus }
                onBlur={ props.onBlur }
                disabled={props.disabled}
            />
        </div>
    );
}

CustomInput.defaultProps = {
    className: '',
    onFocus: undefined,
    onBlur: undefined,
    disabled: false
}

export default CustomInput;