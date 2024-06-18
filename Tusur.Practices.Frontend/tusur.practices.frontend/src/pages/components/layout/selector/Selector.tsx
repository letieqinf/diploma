import { Children, useState } from "react";
import CustomInput from "../CustomInput";

import "./Selector.css";

interface ISelectorOption {
    children: string,
    onMouseDown: React.MouseEventHandler
}

export const SelectorOption: React.FunctionComponent<ISelectorOption> = ( props ) => {
    const { children } = props;

    return (
        children
    );
}

interface ISelector {
    children: React.ReactElement<ISelectorOption> | React.ReactElement<ISelectorOption>[],
    label: any,
    name: string,
    collection: string,
    className: string,
    defaultValue?: string,
    disabled?: boolean,
    onChange: (event: React.SyntheticEvent) => void
}

export const Selector: React.FunctionComponent<ISelector> = ( props ) => {
    const { children, label, name, collection, className, defaultValue, disabled, onChange } = props;

    const [isFocused, setIsFocused] = useState<boolean>(false);
    const [input, setInput] = useState<string>(defaultValue ? defaultValue : ``);

    const changeInput = (event: React.SyntheticEvent) => {
        const target = event.target;
        if (target instanceof HTMLInputElement)
            setInput(target.value);
    }

    const onMouseDown = (
        event: React.MouseEvent<HTMLParagraphElement, MouseEvent>,
        element: React.ReactElement<ISelectorOption>
    ) => {
        setInput(element.props.children);
        element.props.onMouseDown(event);
    }

    return (
        <div className={ `input-selector-container ${ className }` }>
            <CustomInput
                label={ label }
                name={ name }
                collection={ collection }
                value={ input }
                className={ `input` }
                onChange={ (event: React.SyntheticEvent) => { changeInput(event); onChange(event); } }
                onFocus={ () => setIsFocused(true) }
                onBlur={ () => setIsFocused(false) }
                disabled={ disabled }
            />
            {
                isFocused &&
                <div className={ `input-selector` }>
                    {
                        Children.map(children, (child, index) => {
                            if (child.props.children && child.props.children.includes(input)) {
                                return (
                                    <p
                                        onMouseDown={ (event) => onMouseDown(event, child) }
                                        key={ index }
                                    >
                                        { child }
                                    </p>
                                );
                            }
                        })
                    }
                </div>
            }
        </div>
    );
}